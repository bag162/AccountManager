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
                name: "CommentGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APIToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailType = table.Column<int>(type: "int", nullable: false)
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
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APIKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceType = table.Column<int>(type: "int", nullable: false)
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
                    AccountGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProxyGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsefulData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostCommentGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_CommentGroup_PostCommentGroupId",
                        column: x => x.PostCommentGroupId,
                        principalTable: "CommentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstGroupId = table.Column<int>(type: "int", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstAccount_InstAccountGroup_InstGroupId",
                        column: x => x.InstGroupId,
                        principalTable: "InstAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PostGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "InstAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "Follow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecipientAccountId = table.Column<int>(type: "int", nullable: false),
                    SenderAccountId = table.Column<int>(type: "int", nullable: false),
                    FollowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follow_InstAccount_RecipientAccountId",
                        column: x => x.RecipientAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follow_InstAccount_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostURI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredCountLikes = table.Column<int>(type: "int", nullable: false),
                    RequiredCountComments = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PostCommentGroupId = table.Column<int>(type: "int", nullable: false),
                    PostStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_CommentGroup_PostCommentGroupId",
                        column: x => x.PostCommentGroupId,
                        principalTable: "CommentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Post_PostGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PostGroup",
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
                    TaskId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_BASExeption_Task_TaskId",
                        column: x => x.TaskId,
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
                    UsefulData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ProxyId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    RegistrationVerifyResoursesType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerTask_InstAccount_AccountId",
                        column: x => x.AccountId,
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
                        name: "FK_WorkerTask_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostingErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostURI = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    InstPostStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstPost_InstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstPost_Post_PostId",
                        column: x => x.PostId,
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
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    SenderAccountId = table.Column<int>(type: "int", nullable: true),
                    CommentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComment_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PostComment_InstAccount_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostComment_InstPost_PostId",
                        column: x => x.PostId,
                        principalTable: "InstPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    SenderAccountId = table.Column<int>(type: "int", nullable: true),
                    LikeStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLike_InstAccount_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostLike_InstPost_PostId",
                        column: x => x.PostId,
                        principalTable: "InstPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_AccountId",
                table: "BASExeption",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_ProxyId",
                table: "BASExeption",
                column: "ProxyId");

            migrationBuilder.CreateIndex(
                name: "IX_BASExeption_TaskId",
                table: "BASExeption",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostCommentGroupId",
                table: "Comment",
                column: "PostCommentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentGroup_Name",
                table: "CommentGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Follow_RecipientAccountId",
                table: "Follow",
                column: "RecipientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_SenderAccountId",
                table: "Follow",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_InstGroupId",
                table: "InstAccount",
                column: "InstGroupId");

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
                name: "IX_InstPost_AccountId",
                table: "InstPost",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InstPost_PostId",
                table: "InstPost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_InstPost_PostURI",
                table: "InstPost",
                column: "PostURI",
                unique: true,
                filter: "[PostURI] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Post_GroupId",
                table: "Post",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostCommentGroupId",
                table: "Post",
                column: "PostCommentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_CommentId",
                table: "PostComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostId",
                table: "PostComment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_SenderAccountId",
                table: "PostComment",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostGroup_AccountGroupId",
                table: "PostGroup",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PostGroup_Name",
                table: "PostGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_PostId",
                table: "PostLike",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_SenderAccountId",
                table: "PostLike",
                column: "SenderAccountId");

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
                name: "IX_WorkerTask_AccountId",
                table: "WorkerTask",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_ProxyId",
                table: "WorkerTask",
                column: "ProxyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTask_TaskId",
                table: "WorkerTask",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BASExeption");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "SMSActivation");

            migrationBuilder.DropTable(
                name: "WorkerTask");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "InstPost");

            migrationBuilder.DropTable(
                name: "Proxy");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "InstAccount");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "ProxyGroup");

            migrationBuilder.DropTable(
                name: "CommentGroup");

            migrationBuilder.DropTable(
                name: "PostGroup");

            migrationBuilder.DropTable(
                name: "InstAccountGroup");
        }
    }
}
