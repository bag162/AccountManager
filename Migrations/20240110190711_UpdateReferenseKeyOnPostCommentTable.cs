using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateReferenseKeyOnPostCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment");

            migrationBuilder.AlterColumn<int>(
                name: "SenderAccountId",
                table: "PostComment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment");

            migrationBuilder.AlterColumn<int>(
                name: "SenderAccountId",
                table: "PostComment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_InstAccount_SenderAccountId",
                table: "PostComment",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
