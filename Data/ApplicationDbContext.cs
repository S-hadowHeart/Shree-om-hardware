using Microsoft.EntityFrameworkCore;
using shree_om.Models;

namespace shree_om.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Door Handles", Description = "Premium door handle collection" },
                new Category { Id = 2, Name = "Locks", Description = "High-security lock systems" },
                new Category { Id = 3, Name = "Hinges", Description = "Durable hinge solutions" },
                new Category { Id = 4, Name = "Tower Bolts", Description = "Tower bolt collection" },
                new Category { Id = 5, Name = "Accessories", Description = "Hardware accessories" }
            );

            // Seed products (8 matching Figma designs)
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Premium SS304 Door Handle – Chrome", Description = "High-quality stainless steel SS304 door handle with chrome finish. Perfect for residential and commercial applications.", Price = 899, OriginalPrice = 1200, ImageUrl = "/images/handle-chrome.jpg", Stock = 234, Rating = 4.5m, ReviewCount = 156, IsFeatured = true, DiscountPercent = 25, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 2, Name = "Premium SS304 Door Handle – Matt Black", Description = "Sleek matte black door handle for modern interiors. SS304 grade stainless steel.", Price = 899, OriginalPrice = 1200, ImageUrl = "/images/handle-black.jpg", Stock = 234, Rating = 4.5m, ReviewCount = 156, IsFeatured = true, DiscountPercent = 75, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 3, Name = "Premium SS304 Door Handle – Gold", Description = "Elegant gold finish door handle for luxury interiors.", Price = 899, OriginalPrice = 1200, ImageUrl = "/images/handle-gold.jpg", Stock = 234, Rating = 4.5m, ReviewCount = 156, IsFeatured = false, DiscountPercent = 25, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 4, Name = "Heavy Duty Lock Set Premium", Description = "Premium heavy-duty lock set for maximum security. Anti-pick, anti-drill design.", Price = 1250, OriginalPrice = 1800, ImageUrl = "/images/lock-set.jpg", Stock = 1196, Rating = 4.8m, ReviewCount = 93, IsFeatured = true, DiscountPercent = 0, CategoryId = 2, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 5, Name = "Door Handle Matt Black Finish", Description = "Sleek matte black door handle for modern interiors.", Price = 749, OriginalPrice = 749, ImageUrl = "/images/handle-mb.jpg", Stock = 195, Rating = 4.5m, ReviewCount = 267, IsFeatured = true, DiscountPercent = 21, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 6, Name = "Heavy Duty Hinge Set (Pack of 2)", Description = "Industrial-grade hinges. Corrosion-resistant. Sold as a pack of 2.", Price = 599, OriginalPrice = 599, ImageUrl = "/images/hinge-set.jpg", Stock = 1236, Rating = 4.6m, ReviewCount = 421, IsFeatured = true, DiscountPercent = 0, CategoryId = 3, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 7, Name = "Main Door Handle SMD-1012", Description = "Premium main door handle with multiple size and finish options. Ergonomic grip design.", Price = 749, OriginalPrice = 899, ImageUrl = "/images/handle-smd.jpg", Stock = 480, Rating = 4.3m, ReviewCount = 87, IsFeatured = false, DiscountPercent = 11, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) },
                new Product { Id = 8, Name = "Premium Main Door Handle SMD-1007", Description = "Premium main door handle with multiple variants. Crafted for modern aesthetics and long-lasting durability.", Price = 799, OriginalPrice = 899, ImageUrl = "/images/handle-smd2.jpg", Stock = 480, Rating = 4.5m, ReviewCount = 198, IsFeatured = false, DiscountPercent = 11, CategoryId = 1, CreatedAt = new DateTime(2026,1,1) }
            );

            // Seed orders
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderNumber = "ORD-2025-PUD268CBO", CustomerName = "yashimakwana2275", OrderDate = new DateTime(2026,2,15), ItemCount = 1, TotalAmount = 1061, Status = "Pending" },
                new Order { Id = 2, OrderNumber = "ORD-2025-EZ6XZAXH2", CustomerName = "yashimakwana2275", OrderDate = new DateTime(2026,2,13), ItemCount = 1, TotalAmount = 1475, Status = "Pending" },
                new Order { Id = 3, OrderNumber = "ORD-001", CustomerName = "Rajesh Kumar", OrderDate = new DateTime(2025,2,12), ItemCount = 2, TotalAmount = 3597, Status = "Delivered" },
                new Order { Id = 4, OrderNumber = "ORD-002", CustomerName = "Priya Sharma", OrderDate = new DateTime(2025,2,11), ItemCount = 2, TotalAmount = 2250, Status = "Shipped" },
                new Order { Id = 5, OrderNumber = "ORD-003", CustomerName = "Amit Patel", OrderDate = new DateTime(2025,2,10), ItemCount = 1, TotalAmount = 899, Status = "Processing" },
                new Order { Id = 6, OrderNumber = "ORD-004", CustomerName = "Suresh Kumar", OrderDate = new DateTime(2025,2,9), ItemCount = 3, TotalAmount = 4580, Status = "Pending" },
                new Order { Id = 7, OrderNumber = "ORD-005", CustomerName = "Neha Gupta", OrderDate = new DateTime(2025,2,8), ItemCount = 1, TotalAmount = 1798, Status = "Delivered" }
            );
        }
    }
}
