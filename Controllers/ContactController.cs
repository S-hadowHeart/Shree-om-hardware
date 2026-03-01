using Microsoft.AspNetCore.Mvc;
using shree_om.Data;
using shree_om.Models;
using shree_om.Models.ViewModels;
using shree_om.Services;

namespace shree_om.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ContactController(ApplicationDbContext context, IEmailService emailService)
        {
            _context      = context;
            _emailService = emailService;
        }

        // GET: /Contact
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Contact Us – Shree Om Hardware";
            return View(new ContactViewModel());
        }

        // POST: /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var message = new ContactMessage
            {
                FullName = model.FullName,
                Email    = model.Email,
                Phone    = model.Phone,
                Subject  = model.Subject,
                Message  = model.Message,
                SentAt   = DateTime.UtcNow
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // Send thank-you email (swallow errors so the form still succeeds)
            try { await _emailService.SendContactThankYouAsync(model.Email, model.FullName); }
            catch { }

            TempData["ContactSuccess"] = "Thank you! Your message has been sent. We'll get back to you within 24 hours.";
            return RedirectToAction("Index");
        }
    }
}
