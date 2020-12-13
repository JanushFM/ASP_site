using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class deletedUniqueConstraintPainting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Order_PaintingId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaintingId",
                table: "Order",
                column: "PaintingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Order_PaintingId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaintingId",
                table: "Order",
                column: "PaintingId",
                unique: true);
        }
    }
}
