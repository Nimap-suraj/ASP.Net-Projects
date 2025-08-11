using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vidly.Migrations
{
    /// <inheritdoc />
    public partial class addRentak2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "NumberOfAvailable",
                table: "Movies",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
            migrationBuilder.Sql("UPDATE Movies SET NumberOfAvailable = NumberOfStack");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAvailable",
                table: "Movies");
        }
    }
}
