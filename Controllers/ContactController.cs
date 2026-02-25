using Microsoft.AspNetCore.Mvc;
using shree_om.Data;
using shree_om.Models;
using shree_om.Models.ViewModels;

namespace shree_om.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
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

            TempData["ContactSuccess"] = "Thank you! Your message has been sent. We'll get back to you within 24 hours.";
            return RedirectToAction("Index");
        }
    }
}
