using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddPostingImpl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_InstAccount_AccountId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Post_PostId",
                table: "PostComment");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_AccountId",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "PostComment");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "PostComment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "PostComment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderAccountId",
                table: "PostComment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostCommentGroupId",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_CommentGroup_PostGroupId",
                        column: x => x.PostGroupId,
                        principalTable: "CommentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_CommentId",
                table: "PostComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_SenderAccountId",
                table: "PostComment",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostCommentGroupId",
                table: "Post",
                column: "PostCommentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostGroupId",
                table: "Comment",
                column: "PostGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_CommentGroup_PostCommentGroupId",
                table: "Post",
                column: "PostCommentGroupId",
                principalTable: "CommentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Comment_CommentId",
                table: "PostComment",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_InstPost_PostId",
                table: "PostComment",
                column: "PostId",
                principalTable: "InstPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_CommentGroup_PostCommentGroupId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Comment_CommentId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_InstPost_PostId",
                table: "PostComment");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "CommentGroup");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_CommentId",
                table: "PostComment");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_SenderAccountId",
                table: "PostComment");

            migrationBuilder.DropIndex(
                name: "IX_Post_PostCommentGroupId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "PostComment");

            migrationBuilder.DropColumn(
                name: "PostCommentGroupId",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "PostComment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "PostComment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    LikeStatus = table.Column<int>(type: "int", nullable: false),
                    LikeTime = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_AccountId",
                table: "PostComment",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_AccountId",
                table: "PostLike",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_PostId",
                table: "PostLike",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_InstAccount_AccountId",
                table: "PostComment",
                column: "AccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Post_PostId",
                table: "PostComment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
