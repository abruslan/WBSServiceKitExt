using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class ProjectShortNameUndo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "WBS_Projects");
        }
    }
}
