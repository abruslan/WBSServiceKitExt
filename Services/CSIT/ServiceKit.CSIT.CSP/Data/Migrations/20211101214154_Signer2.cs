using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.CSIT.CSP.Data.Migrations
{
    public partial class Signer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Signers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Signers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Signers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Signers");
        }
    }
}
