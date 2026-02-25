using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shree_om.Data;
using shree_om.Models;
using System.Diagnostics;

namespace shree_om.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _context.Products
                .Where(p => p.IsFeatured)
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Take(4)
                .ToListAsync();

            return View(featuredProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
