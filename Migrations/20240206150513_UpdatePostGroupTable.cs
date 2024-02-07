using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdatePostGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup");

            migrationBuilder.AlterColumn<int>(
                name: "AccountGroupId",
                table: "PostGroup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup",
                column: "AccountGroupId",
                principalTable: "InstAccountGroup",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup");

            migrationBuilder.AlterColumn<int>(
                name: "AccountGroupId",
                table: "PostGroup",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostGroup_InstAccountGroup_AccountGroupId",
                table: "PostGroup",
                column: "AccountGroupId",
                principalTable: "InstAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
