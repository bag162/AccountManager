using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateNamingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_InstAccount_SenderAccountId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_InstPost_PostId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "InstPost");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "PostLike");

            migrationBuilder.RenameIndex(
                name: "IX_Like_SenderAccountId",
                table: "PostLike",
                newName: "IX_PostLike_SenderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_PostId",
                table: "PostLike",
                newName: "IX_PostLike_PostId");

            migrationBuilder.AddColumn<int>(
                name: "LikeStatus",
                table: "PostLike",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostLike",
                table: "PostLike",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLike_InstAccount_SenderAccountId",
                table: "PostLike",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLike_InstPost_PostId",
                table: "PostLike",
                column: "PostId",
                principalTable: "InstPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLike_InstAccount_SenderAccountId",
                table: "PostLike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLike_InstPost_PostId",
                table: "PostLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostLike",
                table: "PostLike");

            migrationBuilder.DropColumn(
                name: "LikeStatus",
                table: "PostLike");

            migrationBuilder.RenameTable(
                name: "PostLike",
                newName: "Like");

            migrationBuilder.RenameIndex(
                name: "IX_PostLike_SenderAccountId",
                table: "Like",
                newName: "IX_Like_SenderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLike_PostId",
                table: "Like",
                newName: "IX_Like_PostId");

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "InstPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_InstAccount_SenderAccountId",
                table: "Like",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_InstPost_PostId",
                table: "Like",
                column: "PostId",
                principalTable: "InstPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
