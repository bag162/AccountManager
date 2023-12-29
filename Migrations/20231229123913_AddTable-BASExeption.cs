using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddTableBASExeption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BASExeption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExeptionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExeptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    ProxyId = table.Column<int>(type: "int", nullable: true),
                    DBTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BASExeption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BASExeption_InstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BASExeption_Proxy_ProxyId",
                        column: x => x.ProxyId,
                        principalTable: "Proxy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BASExeption_Task_DBTaskId",
                        column: x => x.DBTaskId,
                        principalTable: "Task",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_AccountId",
                table: "BASExeption",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_DBTaskId",
                table: "BASExeption",
                column: "DBTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_ProxyId",
                table: "BASExeption",
                column: "ProxyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BASExeption");
        }
    }
}
