using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vidly.Migrations
{
    /// <inheritdoc />
    public partial class PopulateMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("INSERT INTO MemberShipType(Id,SigUpFee,DurationInMonth,DiscountRate) VALUES(1,0,0,0)");
            //migrationBuilder.Sql("INSERT INTO MemberShipType(Id,SigUpFee,DurationInMonth,DiscountRate) VALUES(2,30,1,10)");
            //migrationBuilder.Sql("INSERT INTO MemberShipType(Id,SigUpFee,DurationInMonth,DiscountRate) VALUES(3,60,3,15)");
            //migrationBuilder.Sql("INSERT INTO MemberShipType(Id,SigUpFee,DurationInMonth,DiscountRate) VALUES(4,90,6,20)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
