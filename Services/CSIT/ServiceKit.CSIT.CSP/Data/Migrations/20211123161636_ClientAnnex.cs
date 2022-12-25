using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceKit.CSIT.CSP.Data.Migrations
{
    public partial class ClientAnnex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientAnnexId",
                table: "ClientServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientAnnexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAnnexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAnnexes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientServices_ClientAnnexId",
                table: "ClientServices",
                column: "ClientAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAnnexes_ClientId",
                table: "ClientAnnexes",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientServices_ClientAnnexes_ClientAnnexId",
                table: "ClientServices",
                column: "ClientAnnexId",
                principalTable: "ClientAnnexes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientServices_ClientAnnexes_ClientAnnexId",
                table: "ClientServices");

            migrationBuilder.DropTable(
                name: "ClientAnnexes");

            migrationBuilder.DropIndex(
                name: "IX_ClientServices_ClientAnnexId",
                table: "ClientServices");

            migrationBuilder.DropColumn(
                name: "ClientAnnexId",
                table: "ClientServices");
        }
    }
}
