using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class WBS_Log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WBS_ProjectPublications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SyncSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS_ProjectPublications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WBS_ProjectPublications_WBS_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "WBS_Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WBS_ProjectPublications_WBS_SyncSystems_SyncSystemId",
                        column: x => x.SyncSystemId,
                        principalTable: "WBS_SyncSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WBS_SyncLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS_SyncLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WBS_SyncLogs_WBS_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "WBS_Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WBS_SyncLogItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WBS_SyncLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBS_SyncLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WBS_SyncLogItems_WBS_SyncLogs_WBS_SyncLogId",
                        column: x => x.WBS_SyncLogId,
                        principalTable: "WBS_SyncLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WBS_ProjectPublications_ProjectId",
                table: "WBS_ProjectPublications",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WBS_ProjectPublications_SyncSystemId",
                table: "WBS_ProjectPublications",
                column: "SyncSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_WBS_SyncLogItems_WBS_SyncLogId",
                table: "WBS_SyncLogItems",
                column: "WBS_SyncLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WBS_SyncLogs_ProjectId",
                table: "WBS_SyncLogs",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WBS_ProjectPublications");

            migrationBuilder.DropTable(
                name: "WBS_SyncLogItems");

            migrationBuilder.DropTable(
                name: "WBS_SyncLogs");
        }
    }
}
