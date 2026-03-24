using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shree_om.Data;
using shree_om.Models;

namespace shree_om.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ─── DASHBOARD ────────────────────────────────────────────────────────────

        public async Task<IActionResult> Dashboard()
        {
            var totalSales = await _context.Orders.SumAsync(o => o.TotalAmount);
            var totalOrders = await _context.Orders.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            var lowStockProducts = await _context.Products
                .Where(p => p.Stock < 10)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalSales = totalSales;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.RecentOrders = recentOrders;
            ViewBag.LowStockProducts = lowStockProducts;

            return View();
        }

        // ─── CATEGORY MANAGEMENT ─────────────────────────────────────────────────

        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name, string? description)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var cat = new Category { Name = name, Description = description ?? "" };
                _context.Categories.Add(cat);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category added successfully!";
            }
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat != null)
            {
                _context.Categories.Remove(cat);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            return RedirectToAction(nameof(Categories));
        }

        // ─── PRODUCT MANAGEMENT ───────────────────────────────────────────────────

        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public IActionResult AddProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? imageFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                if (product.OriginalPrice == 0) product.OriginalPrice = product.Price;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/images/products/" + fileName;
                }
                else if (string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    product.ImageUrl = "/images/products/dummy.jpg";
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? imageFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Description");

            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct != null && ModelState.IsValid)
            {
                existingProduct.Name = product.Name ?? existingProduct.Name;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Stock = product.Stock;
                existingProduct.Price = product.Price;
                existingProduct.OriginalPrice = product.Price;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    existingProduct.ImageUrl = "/images/products/" + fileName;
                }
                else if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    existingProduct.ImageUrl = product.ImageUrl;
                }

                if (!string.IsNullOrWhiteSpace(product.Description))
                {
                    existingProduct.Description = product.Description;
                }

                existingProduct.Material = product.Material ?? existingProduct.Material;
                existingProduct.Finish = product.Finish ?? existingProduct.Finish;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Products));
        }

        // ─── USER MANAGEMENT ─────────────────────────────────────────────────────

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user, string password)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Role = user.Role;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Users));
        }

        // ─── ORDER MANAGEMENT ────────────────────────────────────────────────────

        public async Task<IActionResult> Orders(string status = "All")
        {
            var orders = status == "All"
                ? await _context.Orders.Include(o => o.OrderItems).OrderByDescending(o => o.OrderDate).ToListAsync()
                : await _context.Orders.Include(o => o.OrderItems).Where(o => o.Status == status).OrderByDescending(o => o.OrderDate).ToListAsync();

            ViewBag.SelectedStatus = status;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Orders));
        }
    }
}
