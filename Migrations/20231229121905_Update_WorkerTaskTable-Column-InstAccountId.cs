using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class Update_WorkerTaskTableColumnInstAccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask");

            migrationBuilder.AlterColumn<int>(
                name: "InstAccountId",
                table: "WorkerTask",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask",
                column: "InstAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask");

            migrationBuilder.AlterColumn<int>(
                name: "InstAccountId",
                table: "WorkerTask",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask",
                column: "InstAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id");
        }
    }
}
