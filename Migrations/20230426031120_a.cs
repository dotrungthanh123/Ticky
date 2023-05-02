using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticky.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Admins_AdminId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Retailers_Admins_AdminId",
                table: "Retailers");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Retailers");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Retailers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Admins_AdminId",
                table: "Events",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Retailers_Admins_AdminId",
                table: "Retailers",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Admins_AdminId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Retailers_Admins_AdminId",
                table: "Retailers");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Retailers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Retailers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Admins_AdminId",
                table: "Events",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Retailers_Admins_AdminId",
                table: "Retailers",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
