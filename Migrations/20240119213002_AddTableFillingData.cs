using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddTableFillingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FillingDataId",
                table: "InstAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FillingData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutMe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    EnableRecomendations = table.Column<bool>(type: "bit", nullable: false),
                    ClosedAccount = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillingData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_FillingDataId",
                table: "InstAccount",
                column: "FillingDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstAccount_FillingData_FillingDataId",
                table: "InstAccount",
                column: "FillingDataId",
                principalTable: "FillingData",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstAccount_FillingData_FillingDataId",
                table: "InstAccount");

            migrationBuilder.DropTable(
                name: "FillingData");

            migrationBuilder.DropIndex(
                name: "IX_InstAccount_FillingDataId",
                table: "InstAccount");

            migrationBuilder.DropColumn(
                name: "FillingDataId",
                table: "InstAccount");
        }
    }
}
