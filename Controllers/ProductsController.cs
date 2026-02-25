using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shree_om.Data;
using shree_om.Models;
using shree_om.Models.ViewModels;

namespace shree_om.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Products or /Products/Index
        public async Task<IActionResult> Index(int? categoryId, decimal? minPrice, decimal? maxPrice, string? sort, string? search)
        {
            var categories = await _context.Categories.ToListAsync();

            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Filter by category
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // Filter by price
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            // Filter by search
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            // Sort
            query = sort switch
            {
                "price_asc"  => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "rating"     => query.OrderByDescending(p => p.Rating),
                "newest"     => query.OrderByDescending(p => p.CreatedAt),
                _            => query.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.Id) // Featured first
            };

            var products = await query.ToListAsync();

            var vm = new ProductFilterViewModel
            {
                Products   = products,
                Categories = categories,
                CategoryId = categoryId,
                MinPrice   = minPrice,
                MaxPrice   = maxPrice,
                Sort       = sort,
                Search     = search,
                TotalCount = products.Count
            };

            return View(vm);
        }

        // GET: /Products/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            // Related products (same category, exclude current)
            var related = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
                .Take(4)
                .ToListAsync();

            ViewData["RelatedProducts"] = related;
            return View(product);
        }
    }
}
