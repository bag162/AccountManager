using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateNamingOnTableFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follow_InstAccount_AccountToFollowId",
                table: "Follow");

            migrationBuilder.DropForeignKey(
                name: "FK_Follow_InstAccount_FollowingAccountId",
                table: "Follow");

            migrationBuilder.RenameColumn(
                name: "FollowingAccountId",
                table: "Follow",
                newName: "SenderAccountId");

            migrationBuilder.RenameColumn(
                name: "AccountToFollowId",
                table: "Follow",
                newName: "RecipientAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_FollowingAccountId",
                table: "Follow",
                newName: "IX_Follow_SenderAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_AccountToFollowId",
                table: "Follow",
                newName: "IX_Follow_RecipientAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_InstAccount_RecipientAccountId",
                table: "Follow",
                column: "RecipientAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_InstAccount_SenderAccountId",
                table: "Follow",
                column: "SenderAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follow_InstAccount_RecipientAccountId",
                table: "Follow");

            migrationBuilder.DropForeignKey(
                name: "FK_Follow_InstAccount_SenderAccountId",
                table: "Follow");

            migrationBuilder.RenameColumn(
                name: "SenderAccountId",
                table: "Follow",
                newName: "FollowingAccountId");

            migrationBuilder.RenameColumn(
                name: "RecipientAccountId",
                table: "Follow",
                newName: "AccountToFollowId");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_SenderAccountId",
                table: "Follow",
                newName: "IX_Follow_FollowingAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_RecipientAccountId",
                table: "Follow",
                newName: "IX_Follow_AccountToFollowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_InstAccount_AccountToFollowId",
                table: "Follow",
                column: "AccountToFollowId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_InstAccount_FollowingAccountId",
                table: "Follow",
                column: "FollowingAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
