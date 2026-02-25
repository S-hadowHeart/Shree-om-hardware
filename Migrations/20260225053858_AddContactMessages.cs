using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shree_om.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Tower bolt collection", "Tower Bolts" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 5, "Hardware accessories", "Accessories" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DiscountPercent", "OriginalPrice", "Rating", "ReviewCount", "Stock" },
                values: new object[] { "High-quality stainless steel SS304 door handle with chrome finish. Perfect for residential and commercial applications.", 25, 1200m, 4.5m, 156, 234 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "DiscountPercent", "ImageUrl", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Stock" },
                values: new object[] { 1, "Sleek matte black door handle for modern interiors. SS304 grade stainless steel.", 75, "/images/handle-black.jpg", "Premium SS304 Door Handle – Matt Black", 1200m, 899m, 4.5m, 156, 234 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "DiscountPercent", "ImageUrl", "IsFeatured", "Name", "OriginalPrice", "Price", "ReviewCount", "Stock" },
                values: new object[] { "Elegant gold finish door handle for luxury interiors.", 25, "/images/handle-gold.jpg", false, "Premium SS304 Door Handle – Gold", 1200m, 899m, 156, 234 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Stock" },
                values: new object[] { 2, "Premium heavy-duty lock set for maximum security. Anti-pick, anti-drill design.", "/images/lock-set.jpg", "Heavy Duty Lock Set Premium", 1800m, 1250m, 4.8m, 93, 1196 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "DiscountPercent", "ImageUrl", "IsFeatured", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Stock" },
                values: new object[,]
                {
                    { 5, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sleek matte black door handle for modern interiors.", 21, "/images/handle-mb.jpg", true, "Door Handle Matt Black Finish", 749m, 749m, 4.5m, 267, 195 },
                    { 6, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Industrial-grade hinges. Corrosion-resistant. Sold as a pack of 2.", 0, "/images/hinge-set.jpg", true, "Heavy Duty Hinge Set (Pack of 2)", 599m, 599m, 4.6m, 421, 1236 },
                    { 7, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium main door handle with multiple size and finish options. Ergonomic grip design.", 11, "/images/handle-smd.jpg", false, "Main Door Handle SMD-1012", 899m, 749m, 4.3m, 87, 480 },
                    { 8, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium main door handle with multiple variants. Crafted for modern aesthetics and long-lasting durability.", 11, "/images/handle-smd2.jpg", false, "Premium Main Door Handle SMD-1007", 899m, 799m, 4.5m, 198, 480 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Complete hardware sets", "Hardware Sets" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DiscountPercent", "OriginalPrice", "Rating", "ReviewCount", "Stock" },
                values: new object[] { "High-quality stainless steel SS304 door handle with chrome finish.", 31, 1299m, 4.6m, 784, 124 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "DiscountPercent", "ImageUrl", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Stock" },
                values: new object[] { 2, "Premium heavy-duty lock set for maximum security.", 0, "/images/lock-set.jpg", "Heavy Duty Lock Set Premium", 1800m, 1250m, 4.8m, 93, 1196 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "DiscountPercent", "ImageUrl", "IsFeatured", "Name", "OriginalPrice", "Price", "ReviewCount", "Stock" },
                values: new object[] { "Sleek matte black door handle for modern interiors.", 21, "/images/handle-black.jpg", true, "Door Handle Matt Black Finish", 749m, 749m, 267, 195 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OriginalPrice", "Price", "Rating", "ReviewCount", "Stock" },
                values: new object[] { 3, "Industrial-grade hinges, sold as a pack of 2.", "/images/hinge-set.jpg", "Heavy Duty Hinge Set (Pack of 2)", 599m, 599m, 4.6m, 421, 1236 });
        }
    }
}
