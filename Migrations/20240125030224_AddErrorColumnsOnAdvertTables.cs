using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddErrorColumnsOnAdvertTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentingErrorMessage",
                table: "AdvertPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LikingErrorMessage",
                table: "AdvertPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "AdvertAccount",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentingErrorMessage",
                table: "AdvertPost");

            migrationBuilder.DropColumn(
                name: "LikingErrorMessage",
                table: "AdvertPost");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "AdvertAccount");
        }
    }
}
