using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class UpdateReferenceNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BASExeption_Task_DBTaskId",
                table: "BASExeption");

            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_InstAccount_InstagramAccountId",
                table: "DBInstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_Post_DBPostId",
                table: "DBInstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_InstAccount_InstAccountGroup_InstAccountGroupId",
                table: "InstAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_Task_DBTaskId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_DBInstPost_DBPostId",
                table: "DBInstPost");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Proxy");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "InstAccount");

            migrationBuilder.DropColumn(
                name: "DBPostId",
                table: "DBInstPost");

            migrationBuilder.RenameColumn(
                name: "InstAccountId",
                table: "WorkerTask",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "DBTaskId",
                table: "WorkerTask",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_InstAccountId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_DBTaskId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_AccountId");

            migrationBuilder.RenameColumn(
                name: "InstAccountGroupId",
                table: "InstAccount",
                newName: "InstGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_InstAccount_InstAccountGroupId",
                table: "InstAccount",
                newName: "IX_InstAccount_InstGroupId");

            migrationBuilder.RenameColumn(
                name: "InstagramAccountId",
                table: "DBInstPost",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "InstAccountId",
                table: "DBInstPost",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DBInstPost_InstagramAccountId",
                table: "DBInstPost",
                newName: "IX_DBInstPost_PostId");

            migrationBuilder.RenameColumn(
                name: "DBTaskId",
                table: "BASExeption",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_BASExeption_DBTaskId",
                table: "BASExeption",
                newName: "IX_BASExeption_TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DBInstPost_AccountId",
                table: "DBInstPost",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BASExeption_Task_TaskId",
                table: "BASExeption",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_InstAccount_AccountId",
                table: "DBInstPost",
                column: "AccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_Post_PostId",
                table: "DBInstPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstAccount_InstAccountGroup_InstGroupId",
                table: "InstAccount",
                column: "InstGroupId",
                principalTable: "InstAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_InstAccount_AccountId",
                table: "WorkerTask",
                column: "AccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_Task_TaskId",
                table: "WorkerTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BASExeption_Task_TaskId",
                table: "BASExeption");

            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_InstAccount_AccountId",
                table: "DBInstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_DBInstPost_Post_PostId",
                table: "DBInstPost");

            migrationBuilder.DropForeignKey(
                name: "FK_InstAccount_InstAccountGroup_InstGroupId",
                table: "InstAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_InstAccount_AccountId",
                table: "WorkerTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTask_Task_TaskId",
                table: "WorkerTask");

            migrationBuilder.DropIndex(
                name: "IX_DBInstPost_AccountId",
                table: "DBInstPost");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "WorkerTask",
                newName: "InstAccountId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "WorkerTask",
                newName: "DBTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_TaskId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_InstAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerTask_AccountId",
                table: "WorkerTask",
                newName: "IX_WorkerTask_DBTaskId");

            migrationBuilder.RenameColumn(
                name: "InstGroupId",
                table: "InstAccount",
                newName: "InstAccountGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_InstAccount_InstGroupId",
                table: "InstAccount",
                newName: "IX_InstAccount_InstAccountGroupId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "DBInstPost",
                newName: "InstagramAccountId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DBInstPost",
                newName: "InstAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DBInstPost_PostId",
                table: "DBInstPost",
                newName: "IX_DBInstPost_InstagramAccountId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "BASExeption",
                newName: "DBTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_BASExeption_TaskId",
                table: "BASExeption",
                newName: "IX_BASExeption_DBTaskId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Proxy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "InstAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DBPostId",
                table: "DBInstPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DBInstPost_DBPostId",
                table: "DBInstPost",
                column: "DBPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_BASExeption_Task_DBTaskId",
                table: "BASExeption",
                column: "DBTaskId",
                principalTable: "Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_InstAccount_InstagramAccountId",
                table: "DBInstPost",
                column: "InstagramAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DBInstPost_Post_DBPostId",
                table: "DBInstPost",
                column: "DBPostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstAccount_InstAccountGroup_InstAccountGroupId",
                table: "InstAccount",
                column: "InstAccountGroupId",
                principalTable: "InstAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_InstAccount_InstAccountId",
                table: "WorkerTask",
                column: "InstAccountId",
                principalTable: "InstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTask_Task_DBTaskId",
                table: "WorkerTask",
                column: "DBTaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
