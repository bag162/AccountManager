using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddInstPostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_InstAccount_AccountId",
                table: "DBInstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_Post_PostId",
                table: "DBInstPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DBInstPost",
                table: "DBInstPost");

            migrationBuilder.RenameTable(
                name: "DBInstPost",
                newName: "InstPost");

            migrationBuilder.RenameIndex(
                name: "IX_DBInstPost_PostId",
                table: "InstPost",
                newName: "IX_InstPost_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_DBInstPost_AccountId",
                table: "InstPost",
                newName: "IX_InstPost_AccountId");

            migrationBuilder.AddColumn<int>(
                name: "InstPostStatus",
                table: "InstPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstPost",
                table: "InstPost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstPost_InstAccount_AccountId",
                table: "InstPost",
                column: "AccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstPost_Post_PostId",
                table: "InstPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstPost_InstAccount_AccountId",
                table: "InstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_InstPost_Post_PostId",
                table: "InstPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstPost",
                table: "InstPost");

            migrationBuilder.DropColumn(
                name: "InstPostStatus",
                table: "InstPost");

            migrationBuilder.RenameTable(
                name: "InstPost",
                newName: "DBInstPost");

            migrationBuilder.RenameIndex(
                name: "IX_InstPost_PostId",
                table: "DBInstPost",
                newName: "IX_DBInstPost_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_InstPost_AccountId",
                table: "DBInstPost",
                newName: "IX_DBInstPost_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DBInstPost",
                table: "DBInstPost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_InstAccount_AccountId",
                table: "DBInstPost",
                column: "AccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_Post_PostId",
                table: "DBInstPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
