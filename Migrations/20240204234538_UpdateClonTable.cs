using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateClonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClonStatus",
                table: "Clon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClonURI",
                table: "Clon",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clon_ClonURI",
                table: "Clon",
                column: "ClonURI",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clon_ClonURI",
                table: "Clon");

            migrationBuilder.DropColumn(
                name: "ClonStatus",
                table: "Clon");

            migrationBuilder.DropColumn(
                name: "ClonURI",
                table: "Clon");
        }
    }
}
