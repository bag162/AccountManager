using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdatenamingCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_CommentGroup_PostGroupId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PostGroupId",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CommentGroupId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentGroupId",
                table: "Comment",
                column: "CommentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_CommentGroup_CommentGroupId",
                table: "Comment",
                column: "CommentGroupId",
                principalTable: "CommentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_CommentGroup_CommentGroupId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_CommentGroupId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentGroupId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostGroupId",
                table: "Comment",
                column: "PostGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_CommentGroup_PostGroupId",
                table: "Comment",
                column: "PostGroupId",
                principalTable: "CommentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
