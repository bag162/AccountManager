using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FBAccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBAccountGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangeIpURI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandartRotationSec = table.Column<int>(type: "int", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProxyGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProxyGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMSActivation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APIURI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APIKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSActivation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UsefulData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FBAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProxyId = table.Column<int>(type: "int", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FBAccount_Proxy_ProxyId",
                        column: x => x.ProxyId,
                        principalTable: "Proxy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FBAccount_ProxyId",
                table: "FBAccount",
                column: "ProxyId");

            migrationBuilder.CreateIndex(
                name: "IX_FBAccountGroup_Name",
                table: "FBAccountGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProxyGroup_Name",
                table: "ProxyGroup",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FBAccount");

            migrationBuilder.DropTable(
                name: "FBAccountGroup");

            migrationBuilder.DropTable(
                name: "ProxyGroup");

            migrationBuilder.DropTable(
                name: "SMSActivation");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Proxy");
        }
    }
}
