using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASAccountManager.Migrations
{
    public partial class AddTimeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StandartRotationSec",
                table: "Proxy");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkerTask",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Task",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SMSActivation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Proxy",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PostLike",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Post",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "InstPost",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Follow",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Email",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Comment",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkerTask");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SMSActivation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Proxy");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PostLike");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "InstPost");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Follow");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "StandartRotationSec",
                table: "Proxy",
                type: "int",
                nullable: true);
        }
    }
}
