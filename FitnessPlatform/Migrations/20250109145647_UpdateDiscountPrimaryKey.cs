using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adauga DiscountId ca cheie primara auto-incrementata
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Discounts",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                 name: "PK_Discounts",
                 table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Discounts");
        }
    }
}
