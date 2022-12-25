using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.CSIT.CSP.Data.Migrations
{
    public partial class YearPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "YearPrice",
                table: "ClientServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearPrice",
                table: "ClientServices");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Clients");
        }
    }
}
