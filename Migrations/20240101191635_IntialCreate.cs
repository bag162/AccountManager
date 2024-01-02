using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class IntialCreate : Migration
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
                name: "PostGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostGroup", x => x.Id);
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
                    InstAccountGroupId = table.Column<int>(type: "int", nullable: false),
                    PostGroupId = table.Column<int>(type: "int", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstAccount_InstAccountGroup_InstAccountGroupId",
                        column: x => x.InstAccountGroupId,
                        principalTable: "InstAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InstAccount_PostGroup_PostGroupId",
                        column: x => x.PostGroupId,
                        principalTable: "PostGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostURI = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RequiredCountLikes = table.Column<int>(type: "int", nullable: true),
                    RequiredCountComments = table.Column<int>(type: "int", nullable: true),
                    PostStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_PostGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PostGroup",
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
                    ProxyGroupId = table.Column<int>(type: "int", nullable: false),
                    ProxyStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proxy_ProxyGroup_ProxyGroupId",
                        column: x => x.ProxyGroupId,
                        principalTable: "ProxyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DBInstPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DBPostId = table.Column<int>(type: "int", nullable: false),
                    InstAccountId = table.Column<int>(type: "int", nullable: false),
                    InstagramAccountId = table.Column<int>(type: "int", nullable: false),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBInstPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DBInstPost_InstAccount_InstagramAccountId",
                        column: x => x.InstagramAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DBInstPost_Post_DBPostId",
                        column: x => x.DBPostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CommentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComment_InstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostComment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    LikeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LikeStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLike_InstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostLike_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "WorkerTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InstAccountId = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_DBInstPost_DBPostId",
                table: "DBInstPost",
                column: "DBPostId");

            migrationBuilder.CreateIndex(
                name: "IX_DBInstPost_InstagramAccountId",
                table: "DBInstPost",
                column: "InstagramAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_InstAccountGroupId",
                table: "InstAccount",
                column: "InstAccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_Login",
                table: "InstAccount",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_PostGroupId",
                table: "InstAccount",
                column: "PostGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InstAccountGroup_Name",
                table: "InstAccountGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_GroupId",
                table: "Post",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostURI",
                table: "Post",
                column: "PostURI",
                unique: true,
                filter: "[PostURI] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_AccountId",
                table: "PostComment",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostId",
                table: "PostComment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostGroup_Name",
                table: "PostGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_AccountId",
                table: "PostLike",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_PostId",
                table: "PostLike",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Proxy_ProxyGroupId",
                table: "Proxy",
                column: "ProxyGroupId");

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
                name: "BASExeption");

            migrationBuilder.DropTable(
                name: "DBInstPost");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "SMSActivation");

            migrationBuilder.DropTable(
                name: "WorkerTask");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "InstAccount");

            migrationBuilder.DropTable(
                name: "Proxy");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "InstAccountGroup");

            migrationBuilder.DropTable(
                name: "PostGroup");

            migrationBuilder.DropTable(
                name: "ProxyGroup");
        }
    }
}
