using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddClonImpl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClonId",
                table: "InstAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FillingDataId = table.Column<int>(type: "int", nullable: true),
                    PostGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clon_FillingData_FillingDataId",
                        column: x => x.FillingDataId,
                        principalTable: "FillingData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clon_PostGroup_PostGroupId",
                        column: x => x.PostGroupId,
                        principalTable: "PostGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstAccount_ClonId",
                table: "InstAccount",
                column: "ClonId");

            migrationBuilder.CreateIndex(
                name: "IX_Clon_FillingDataId",
                table: "Clon",
                column: "FillingDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Clon_PostGroupId",
                table: "Clon",
                column: "PostGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstAccount_Clon_ClonId",
                table: "InstAccount",
                column: "ClonId",
                principalTable: "Clon",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstAccount_Clon_ClonId",
                table: "InstAccount");

            migrationBuilder.DropTable(
                name: "Clon");

            migrationBuilder.DropIndex(
                name: "IX_InstAccount_ClonId",
                table: "InstAccount");

            migrationBuilder.DropColumn(
                name: "ClonId",
                table: "InstAccount");
        }
    }
}
