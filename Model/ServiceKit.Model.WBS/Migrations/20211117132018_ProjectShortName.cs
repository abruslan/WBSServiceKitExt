using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class ProjectShortName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectShortName",
                table: "WBS_Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Full1CName",
                table: "WBS_RequestProjectItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Full1CName",
                table: "WBS_RequestItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectShortName",
                table: "WBS_Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectShortName",
                table: "WBS_Requests");

            migrationBuilder.DropColumn(
                name: "Full1CName",
                table: "WBS_RequestProjectItems");

            migrationBuilder.DropColumn(
                name: "Full1CName",
                table: "WBS_RequestItems");

            migrationBuilder.DropColumn(
                name: "ProjectShortName",
                table: "WBS_Projects");
        }
    }
}
