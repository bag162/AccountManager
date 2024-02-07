using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddClonGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostGroup_Name",
                table: "PostGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PostGroup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ClonGroupId",
                table: "Clon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClonGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClonGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clon_ClonGroupId",
                table: "Clon",
                column: "ClonGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClonGroup_Name",
                table: "ClonGroup",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clon_ClonGroup_ClonGroupId",
                table: "Clon",
                column: "ClonGroupId",
                principalTable: "ClonGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clon_ClonGroup_ClonGroupId",
                table: "Clon");

            migrationBuilder.DropTable(
                name: "ClonGroup");

            migrationBuilder.DropIndex(
                name: "IX_Clon_ClonGroupId",
                table: "Clon");

            migrationBuilder.DropColumn(
                name: "ClonGroupId",
                table: "Clon");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PostGroup",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PostGroup_Name",
                table: "PostGroup",
                column: "Name",
                unique: true);
        }
    }
}
