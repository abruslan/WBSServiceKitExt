using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.CSIT.CSP.Data.Migrations
{
    public partial class LogSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Source",
                table: "Logs",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Logs");
        }
    }
}
