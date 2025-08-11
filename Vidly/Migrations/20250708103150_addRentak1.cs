using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vidly.Migrations
{
    /// <inheritdoc />
    public partial class addRentak1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Rental",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_CustomerId",
                table: "Rental",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_MovieId",
                table: "Rental",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Customers_CustomerId",
                table: "Rental",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Movies_MovieId",
                table: "Rental",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Customers_CustomerId",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Movies_MovieId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_CustomerId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_MovieId",
                table: "Rental");

            migrationBuilder.AlterColumn<string>(
                name: "MovieId",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
