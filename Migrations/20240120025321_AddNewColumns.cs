using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Post",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FillingData",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Name",
                table: "Post",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FillingData_Name",
                table: "FillingData",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Post_Name",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_FillingData_Name",
                table: "FillingData");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FillingData");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
