using System.Net;
using System.Net.Mail;

namespace shree_om.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string toEmail, string toName, string verificationLink);
        Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetLink);
        Task SendContactThankYouAsync(string toEmail, string toName);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly string _host;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly string _password;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _host        = config["EmailSettings:SmtpHost"]    ?? "smtp.gmail.com";
            _port        = int.Parse(config["EmailSettings:SmtpPort"] ?? "587");
            _senderEmail = config["EmailSettings:SenderEmail"] ?? "";
            _senderName  = config["EmailSettings:SenderName"]  ?? "Shree Om Hardware";
            _password    = config["EmailSettings:Password"]    ?? "";
        }

        private SmtpClient CreateClient()
        {
            return new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_senderEmail, _password),
                EnableSsl   = true
            };
        }

        private MailMessage BaseMessage(string toEmail, string toName, string subject)
        {
            var msg = new MailMessage
            {
                From       = new MailAddress(_senderEmail, _senderName),
                Subject    = subject,
                IsBodyHtml = true
            };
            msg.To.Add(new MailAddress(toEmail, toName));
            return msg;
        }

        public async Task SendVerificationEmailAsync(string toEmail, string toName, string verificationLink)
        {
            using var client = CreateClient();
            using var msg = BaseMessage(toEmail, toName, "Verify your Shree Om Hardware account");

            msg.Body = $@"
<!DOCTYPE html>
<html>
<body style=""margin:0;padding:0;background:#f5f5f5;font-family:Arial,sans-serif;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
    <tr><td align=""center"" style=""padding:40px 10px;"">
      <table width=""560"" cellpadding=""0"" cellspacing=""0"" style=""background:#fff;border-radius:12px;overflow:hidden;box-shadow:0 2px 12px rgba(0,0,0,.08);"">
        <tr><td style=""background:#1a2332;padding:32px;text-align:center;"">
          <div style=""display:inline-flex;align-items:center;gap:12px;"">
            <div style=""width:44px;height:44px;background:#c8994a;border-radius:8px;display:inline-block;line-height:44px;text-align:center;font-weight:700;font-size:18px;color:#fff;"">SO</div>
            <span style=""color:#fff;font-size:22px;font-weight:700;"">Shree Om Hardware</span>
          </div>
        </td></tr>
        <tr><td style=""padding:40px 40px 32px;"">
          <h2 style=""margin:0 0 12px;color:#1a2332;font-size:22px;"">Verify your email address</h2>
          <p style=""color:#555;line-height:1.6;margin:0 0 24px;"">Hi <strong>{toName}</strong>,<br><br>
          Thanks for creating a Shree Om Hardware account! Please verify your email address to activate your account.</p>
          <div style=""text-align:center;margin:32px 0;"">
            <a href=""{verificationLink}"" style=""background:#c8994a;color:#fff;padding:14px 36px;border-radius:8px;text-decoration:none;font-weight:700;font-size:16px;display:inline-block;"">
              ✓ Verify Email Address
            </a>
          </div>
          <p style=""color:#888;font-size:13px;line-height:1.6;"">This link expires in <strong>24 hours</strong>. If you didn't create an account, you can safely ignore this email.</p>
          <hr style=""border:none;border-top:1px solid #eee;margin:28px 0;"">
          <p style=""color:#aaa;font-size:12px;"">Having trouble? Copy and paste this URL into your browser:<br>
          <a href=""{verificationLink}"" style=""color:#c8994a;word-break:break-all;font-size:11px;"">{verificationLink}</a></p>
        </td></tr>
        <tr><td style=""background:#f9f9f9;padding:20px 40px;text-align:center;border-top:1px solid #eee;"">
          <p style=""color:#aaa;font-size:12px;margin:0;"">© 2026 Shree Om Hardware · Made in India 🇮🇳</p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
            await client.SendMailAsync(msg);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetLink)
        {
            using var client = CreateClient();
            using var msg = BaseMessage(toEmail, toName, "Reset your Shree Om Hardware password");

            msg.Body = $@"
<!DOCTYPE html>
<html>
<body style=""margin:0;padding:0;background:#f5f5f5;font-family:Arial,sans-serif;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
    <tr><td align=""center"" style=""padding:40px 10px;"">
      <table width=""560"" cellpadding=""0"" cellspacing=""0"" style=""background:#fff;border-radius:12px;overflow:hidden;box-shadow:0 2px 12px rgba(0,0,0,.08);"">
        <tr><td style=""background:#1a2332;padding:32px;text-align:center;"">
          <div>
            <div style=""width:44px;height:44px;background:#c8994a;border-radius:8px;display:inline-block;line-height:44px;text-align:center;font-weight:700;font-size:18px;color:#fff;"">SO</div>
            <span style=""color:#fff;font-size:22px;font-weight:700;margin-left:12px;"">Shree Om Hardware</span>
          </div>
        </td></tr>
        <tr><td style=""padding:40px 40px 32px;"">
          <h2 style=""margin:0 0 12px;color:#1a2332;font-size:22px;"">Password Reset Request</h2>
          <p style=""color:#555;line-height:1.6;margin:0 0 24px;"">Hi <strong>{toName}</strong>,<br><br>
          We received a request to reset the password for your Shree Om Hardware account. Click the button below to create a new password.</p>
          <div style=""text-align:center;margin:32px 0;"">
            <a href=""{resetLink}"" style=""background:#c8994a;color:#fff;padding:14px 36px;border-radius:8px;text-decoration:none;font-weight:700;font-size:16px;display:inline-block;"">
              🔑 Reset My Password
            </a>
          </div>
          <p style=""color:#e74c3c;font-size:13px;font-weight:600;"">⏰ This link expires in 1 hour.</p>
          <p style=""color:#888;font-size:13px;line-height:1.6;"">If you didn't request a password reset, please ignore this email — your password will remain unchanged.</p>
          <hr style=""border:none;border-top:1px solid #eee;margin:28px 0;"">
          <p style=""color:#aaa;font-size:12px;"">Having trouble? Copy and paste this URL into your browser:<br>
          <a href=""{resetLink}"" style=""color:#c8994a;word-break:break-all;font-size:11px;"">{resetLink}</a></p>
        </td></tr>
        <tr><td style=""background:#f9f9f9;padding:20px 40px;text-align:center;border-top:1px solid #eee;"">
          <p style=""color:#aaa;font-size:12px;margin:0;"">© 2026 Shree Om Hardware · Made in India 🇮🇳</p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
            await client.SendMailAsync(msg);
        }

        public async Task SendContactThankYouAsync(string toEmail, string toName)
        {
            using var client = CreateClient();
            using var msg = BaseMessage(toEmail, toName, "Thank you for contacting Shree Om Hardware");

            msg.Body = $@"
<!DOCTYPE html>
<html>
<body style=""margin:0;padding:0;background:#f5f5f5;font-family:Arial,sans-serif;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
    <tr><td align=""center"" style=""padding:40px 10px;"">
      <table width=""560"" cellpadding=""0"" cellspacing=""0"" style=""background:#fff;border-radius:12px;overflow:hidden;box-shadow:0 2px 12px rgba(0,0,0,.08);"">
        <tr><td style=""background:#1a2332;padding:32px;text-align:center;"">
          <div>
            <div style=""width:44px;height:44px;background:#c8994a;border-radius:8px;display:inline-block;line-height:44px;text-align:center;font-weight:700;font-size:18px;color:#fff;"">SO</div>
            <span style=""color:#fff;font-size:22px;font-weight:700;margin-left:12px;"">Shree Om Hardware</span>
          </div>
        </td></tr>
        <tr><td style=""padding:40px 40px 32px;"">
          <div style=""text-align:center;margin-bottom:24px;"">
            <div style=""width:64px;height:64px;background:#e8f5e9;border-radius:50%;display:inline-flex;align-items:center;justify-content:center;margin-bottom:16px;"">
              <span style=""font-size:32px;"">✓</span>
            </div>
          </div>
          <h2 style=""margin:0 0 12px;color:#1a2332;font-size:22px;text-align:center;"">Thank You, {toName}!</h2>
          <p style=""color:#555;line-height:1.6;text-align:center;margin:0 0 24px;"">
            We've received your message and our team will get back to you within <strong>24 hours</strong>.
          </p>
          <div style=""background:#fdf8f0;border-left:4px solid #c8994a;padding:16px 20px;border-radius:0 8px 8px 0;margin:24px 0;"">
            <p style=""margin:0;color:#555;font-size:14px;"">📞 For urgent inquiries, call us at <strong>+91-98765-43210</strong><br>
            📧 Or email us directly at <a href=""mailto:info@shreeomhardware.com"" style=""color:#c8994a;"">info@shreeomhardware.com</a></p>
          </div>
          <p style=""color:#888;font-size:13px;line-height:1.6;"">
            In the meantime, feel free to browse our <a href=""http://localhost:5180/Products"" style=""color:#c8994a;"">products catalogue</a>.
          </p>
        </td></tr>
        <tr><td style=""background:#f9f9f9;padding:20px 40px;text-align:center;border-top:1px solid #eee;"">
          <p style=""color:#aaa;font-size:12px;margin:0;"">© 2026 Shree Om Hardware · Made in India 🇮🇳</p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
            await client.SendMailAsync(msg);
        }
    }
}
