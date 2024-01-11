using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateNameingIntablePostComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_CommentGroup_CommentGroupId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "CommentGroupId",
                table: "Comment",
                newName: "PostCommentGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CommentGroupId",
                table: "Comment",
                newName: "IX_Comment_PostCommentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_CommentGroup_PostCommentGroupId",
                table: "Comment",
                column: "PostCommentGroupId",
                principalTable: "CommentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_CommentGroup_PostCommentGroupId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "PostCommentGroupId",
                table: "Comment",
                newName: "CommentGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostCommentGroupId",
                table: "Comment",
                newName: "IX_Comment_CommentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_CommentGroup_CommentGroupId",
                table: "Comment",
                column: "CommentGroupId",
                principalTable: "CommentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
