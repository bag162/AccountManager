using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailType = table.Column<int>(type: "int", nullable: false),
                    APIToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailDomain = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstAccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstAccountGroup", x => x.Id);
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
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    ClientTaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    UsefulData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProxyGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstAccount_InstAccountGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "InstAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ProxyStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proxy_ProxyGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ProxyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InstAccountId = table.Column<int>(type: "int", nullable: true),
                    ProxyId = table.Column<int>(type: "int", nullable: false),
                    DBTaskId = table.Column<int>(type: "int", nullable: false),
                    UsefulData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    RegistrationVerifyResoursesType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerTask_InstAccount_InstAccountId",
                        column: x => x.InstAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerTask_Proxy_ProxyId",
                        column: x => x.ProxyId,
                        principalTable: "Proxy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerTask_Task_DBTaskId",
                        column: x => x.DBTaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_GroupId",
                table: "InstAccount",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_Login",
                table: "InstAccount",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstAccountGroup_Name",
                table: "InstAccountGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proxy_GroupId",
                table: "Proxy",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProxyGroup_Name",
                table: "ProxyGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_DBTaskId",
                table: "WorkerTask",
                column: "DBTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_InstAccountId",
                table: "WorkerTask",
                column: "InstAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_ProxyId",
                table: "WorkerTask",
                column: "ProxyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "SMSActivation");

            migrationBuilder.DropTable(
                name: "WorkerTask");

            migrationBuilder.DropTable(
                name: "InstAccount");

            migrationBuilder.DropTable(
                name: "Proxy");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "InstAccountGroup");

            migrationBuilder.DropTable(
                name: "ProxyGroup");
        }
    }
}
