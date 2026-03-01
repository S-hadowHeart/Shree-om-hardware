using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shree_om.Data;
using shree_om.Models;
using shree_om.Models.ViewModels;
using shree_om.Services;
using System.Security.Claims;

namespace shree_om.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AccountController(ApplicationDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context      = context;
            _emailService = emailService;
            _config       = config;
        }

        // ─── LOGIN ────────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (!user.IsEmailVerified)
            {
                TempData["UnverifiedEmail"] = user.Email;
                ModelState.AddModelError(string.Empty, "Please verify your email before logging in.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,  user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,  user.Role)
            };
            var identity   = new ClaimsIdentity(claims, "CookieAuth");
            var authProps  = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc   = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identity), authProps);
            HttpContext.Session.SetString("UserName",  user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ─── REGISTER ─────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            var token = Guid.NewGuid().ToString("N");
            var user = new User
            {
                FullName                    = model.FullName,
                Email                       = model.Email,
                PhoneNumber                 = model.PhoneNumber,
                PasswordHash               = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt                   = DateTime.UtcNow,
                Role                        = "Customer",
                IsEmailVerified             = false,
                EmailVerificationToken      = token,
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var appUrl = _config["AppUrl"] ?? "http://localhost:5180";
            var verifyLink = $"{appUrl}/Account/VerifyEmail?token={token}";

            try { await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verifyLink); }
            catch { /* Swallow email errors so registration isn't blocked */ }

            TempData["SuccessMessage"] = "Account created! Please check your email and click the verification link before logging in.";
            return RedirectToAction("Login");
        }

        // ─── VERIFY EMAIL ─────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewData["VerifyStatus"] = "invalid";
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

            if (user == null)
            {
                ViewData["VerifyStatus"] = "invalid";
                return View();
            }

            if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
            {
                ViewData["VerifyStatus"] = "expired";
                ViewData["ExpiredEmail"] = user.Email;
                return View();
            }

            user.IsEmailVerified              = true;
            user.EmailVerificationToken       = null;
            user.EmailVerificationTokenExpiry = null;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "✅ Email verified successfully! You can now log in.";
            return RedirectToAction("Login");
        }

        // ─── RESEND VERIFICATION ──────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerification(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsEmailVerified);
            if (user != null)
            {
                user.EmailVerificationToken       = Guid.NewGuid().ToString("N");
                user.EmailVerificationTokenExpiry  = DateTime.UtcNow.AddHours(24);
                await _context.SaveChangesAsync();

                var appUrl     = _config["AppUrl"] ?? "http://localhost:5180";
                var verifyLink = $"{appUrl}/Account/VerifyEmail?token={user.EmailVerificationToken}";

                try { await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, verifyLink); }
                catch { }
            }

            TempData["SuccessMessage"] = "If that email is registered and unverified, a new verification email has been sent.";
            return RedirectToAction("Login");
        }

        // ─── FORGOT PASSWORD ──────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null)
            {
                user.PasswordResetToken       = Guid.NewGuid().ToString("N");
                user.PasswordResetTokenExpiry  = DateTime.UtcNow.AddHours(1);
                await _context.SaveChangesAsync();

                var appUrl    = _config["AppUrl"] ?? "http://localhost:5180";
                var resetLink = $"{appUrl}/Account/ResetPassword?token={user.PasswordResetToken}&email={Uri.EscapeDataString(user.Email)}";

                try { await _emailService.SendPasswordResetEmailAsync(user.Email, user.FullName, resetLink); }
                catch { }
            }

            return RedirectToAction("ForgotPasswordConfirmation", new { email = model.Email });
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        // ─── RESET PASSWORD ───────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == email && u.PasswordResetToken == token);

            if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "This password reset link is invalid or has expired. Please request a new one.";
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == model.Email && u.PasswordResetToken == model.Token);

            if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "This password reset link is invalid or has expired.";
                return RedirectToAction("ForgotPassword");
            }

            user.PasswordHash          = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.PasswordResetToken    = null;
            user.PasswordResetTokenExpiry = null;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "✅ Password reset successfully! Please log in with your new password.";
            return RedirectToAction("Login");
        }

        // ─── LOGOUT ───────────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
