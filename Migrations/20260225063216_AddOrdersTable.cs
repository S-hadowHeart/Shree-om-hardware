using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shree_om.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "ItemCount", "OrderDate", "OrderNumber", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "yashimakwana2275", 1, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-2025-PUD268CBO", "Pending", 1061m },
                    { 2, "yashimakwana2275", 1, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-2025-EZ6XZAXH2", "Pending", 1475m },
                    { 3, "Rajesh Kumar", 2, new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-001", "Delivered", 3597m },
                    { 4, "Priya Sharma", 2, new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-002", "Shipped", 2250m },
                    { 5, "Amit Patel", 1, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-003", "Processing", 899m },
                    { 6, "Suresh Kumar", 3, new DateTime(2025, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-004", "Pending", 4580m },
                    { 7, "Neha Gupta", 1, new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORD-005", "Delivered", 1798m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
