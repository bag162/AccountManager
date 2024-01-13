using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddNewTableFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountToFollowId = table.Column<int>(type: "int", nullable: false),
                    FollowingAccountId = table.Column<int>(type: "int", nullable: false),
                    FollowStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follow_InstAccount_AccountToFollowId",
                        column: x => x.AccountToFollowId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follow_InstAccount_FollowingAccountId",
                        column: x => x.FollowingAccountId,
                        principalTable: "InstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follow_AccountToFollowId",
                table: "Follow",
                column: "AccountToFollowId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowingAccountId",
                table: "Follow",
                column: "FollowingAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follow");
        }
    }
}
