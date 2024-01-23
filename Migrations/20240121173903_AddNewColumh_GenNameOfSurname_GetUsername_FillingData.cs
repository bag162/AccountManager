using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddNewColumh_GenNameOfSurname_GetUsername_FillingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOrSurnameGenString",
                table: "FillingData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsernameGenString",
                table: "FillingData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOrSurnameGenString",
                table: "FillingData");

            migrationBuilder.DropColumn(
                name: "UsernameGenString",
                table: "FillingData");
        }
    }
}
