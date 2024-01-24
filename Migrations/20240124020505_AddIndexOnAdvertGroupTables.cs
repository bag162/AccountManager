using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddIndexOnAdvertGroupTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvertPostGroup",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvertAccountGroup",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertPostGroup_Name",
                table: "AdvertPostGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertAccountGroup_Name",
                table: "AdvertAccountGroup",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdvertPostGroup_Name",
                table: "AdvertPostGroup");

            migrationBuilder.DropIndex(
                name: "IX_AdvertAccountGroup_Name",
                table: "AdvertAccountGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvertPostGroup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AdvertAccountGroup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
