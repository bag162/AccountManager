using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddNewAdvertTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertAccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertAccountGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvertPostGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertPostGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvertAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdvertAccountStatus = table.Column<int>(type: "int", nullable: false),
                    AdvertAccountGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertAccount_AdvertAccountGroup_AdvertAccountGroupId",
                        column: x => x.AdvertAccountGroupId,
                        principalTable: "AdvertAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdvertPostLikeStatus = table.Column<int>(type: "int", nullable: false),
                    AdvertPostCommentStatus = table.Column<int>(type: "int", nullable: false),
                    AdvertPostGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertPost_AdvertPostGroup_AdvertPostGroupId",
                        column: x => x.AdvertPostGroupId,
                        principalTable: "AdvertPostGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertAccount_AdvertAccountGroupId",
                table: "AdvertAccount",
                column: "AdvertAccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertPost_AdvertPostGroupId",
                table: "AdvertPost",
                column: "AdvertPostGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertAccount");

            migrationBuilder.DropTable(
                name: "AdvertPost");

            migrationBuilder.DropTable(
                name: "AdvertAccountGroup");

            migrationBuilder.DropTable(
                name: "AdvertPostGroup");
        }
    }
}
