using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class WBS_ProjectRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "WBS_Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "WBS_Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WBS_RequestProjectItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    OrderNum = table.Column<int>(type: "int", nullable: false),
                    ShortCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS_RequestProjectItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WBS_RequestProjectItems_WBS_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "WBS_Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WBS_RequestProjectItems_RequestId",
                table: "WBS_RequestProjectItems",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WBS_RequestProjectItems");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "WBS_Requests");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "WBS_Items");
        }
    }
}
