using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateTablePostRemoveAccountPinnedAccountAddAccountGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstAccount_PostGroup_PostGroupId",
                table: "InstAccount");

            migrationBuilder.DropIndex(
                name: "IX_InstAccount_PostGroupId",
                table: "InstAccount");

            migrationBuilder.DropColumn(
                name: "PostGroupId",
                table: "InstAccount");

            migrationBuilder.AddColumn<int>(
                name: "AccountGroupId",
                table: "PostGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostGroup_AccountGroupId",
                table: "PostGroup",
                column: "AccountGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup",
                column: "AccountGroupId",
                principalTable: "InstAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup");

            migrationBuilder.DropIndex(
                name: "IX_PostGroup_AccountGroupId",
                table: "PostGroup");

            migrationBuilder.DropColumn(
                name: "AccountGroupId",
                table: "PostGroup");

            migrationBuilder.AddColumn<int>(
                name: "PostGroupId",
                table: "InstAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_PostGroupId",
                table: "InstAccount",
                column: "PostGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstAccount_PostGroup_PostGroupId",
                table: "InstAccount",
                column: "PostGroupId",
                principalTable: "PostGroup",
                principalColumn: "Id");
        }
    }
}
