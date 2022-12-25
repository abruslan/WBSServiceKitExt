using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class WBS_ProjectItem_Full1CName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectShortName",
                table: "WBS_Requests");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "WBS_RequestProjectItems");

            migrationBuilder.DropColumn(
                name: "ProjectShortName",
                table: "WBS_Projects");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "WBS_ProjectItems",
                newName: "Full1CName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Full1CName",
                table: "WBS_ProjectItems",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "ProjectShortName",
                table: "WBS_Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "WBS_RequestProjectItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectShortName",
                table: "WBS_Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
