using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateSchedulerTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_SchedulerTask_SchedulerTaskId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_SchedulerTaskId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "SchedulerTaskId",
                table: "Task");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "SchedulerTask",
                newName: "LastStart");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SchedulerTask",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CurrentTaskPositionIndex",
                table: "SchedulerTask",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskIds",
                table: "SchedulerTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeBetweenLaunchesMinutes",
                table: "SchedulerTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulerTask_Name",
                table: "SchedulerTask",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchedulerTask_Name",
                table: "SchedulerTask");

            migrationBuilder.DropColumn(
                name: "CurrentTaskPositionIndex",
                table: "SchedulerTask");

            migrationBuilder.DropColumn(
                name: "TaskIds",
                table: "SchedulerTask");

            migrationBuilder.DropColumn(
                name: "TimeBetweenLaunchesMinutes",
                table: "SchedulerTask");

            migrationBuilder.RenameColumn(
                name: "LastStart",
                table: "SchedulerTask",
                newName: "StartTime");

            migrationBuilder.AddColumn<int>(
                name: "SchedulerTaskId",
                table: "Task",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SchedulerTask",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
