using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddSchedulerImpl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchedulerTaskId",
                table: "Task",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchedulerTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartupType = table.Column<int>(type: "int", nullable: false),
                    SchedulerTaskStatus = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulerTask", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_SchedulerTaskId",
                table: "Task",
                column: "SchedulerTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_SchedulerTask_SchedulerTaskId",
                table: "Task",
                column: "SchedulerTaskId",
                principalTable: "SchedulerTask",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_SchedulerTask_SchedulerTaskId",
                table: "Task");

            migrationBuilder.DropTable(
                name: "SchedulerTask");

            migrationBuilder.DropIndex(
                name: "IX_Task_SchedulerTaskId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "SchedulerTaskId",
                table: "Task");
        }
    }
}
