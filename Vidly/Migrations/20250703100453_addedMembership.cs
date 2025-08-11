using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vidly.Migrations
{
    /// <inheritdoc />
    public partial class addedMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "MemberShipTypeId",
                table: "Customers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "MemberShipType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    SigUpFee = table.Column<short>(type: "smallint", nullable: false),
                    DurationInMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    DiscountRate = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberShipType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MemberShipTypeId",
                table: "Customers",
                column: "MemberShipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_MemberShipType_MemberShipTypeId",
                table: "Customers",
                column: "MemberShipTypeId",
                principalTable: "MemberShipType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_MemberShipType_MemberShipTypeId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "MemberShipType");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MemberShipTypeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MemberShipTypeId",
                table: "Customers");
        }
    }
}
