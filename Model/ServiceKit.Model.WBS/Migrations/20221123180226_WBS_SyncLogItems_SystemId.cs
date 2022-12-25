using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.Model.WBS.Migrations
{
    public partial class WBS_SyncLogItems_SystemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SyncSystemId",
                table: "WBS_SyncLogItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WBS_SyncLogItems_SyncSystemId",
                table: "WBS_SyncLogItems",
                column: "SyncSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_WBS_SyncLogItems_WBS_SyncSystems_SyncSystemId",
                table: "WBS_SyncLogItems",
                column: "SyncSystemId",
                principalTable: "WBS_SyncSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WBS_SyncLogItems_WBS_SyncSystems_SyncSystemId",
                table: "WBS_SyncLogItems");

            migrationBuilder.DropIndex(
                name: "IX_WBS_SyncLogItems_SyncSystemId",
                table: "WBS_SyncLogItems");

            migrationBuilder.DropColumn(
                name: "SyncSystemId",
                table: "WBS_SyncLogItems");
        }
    }
}
