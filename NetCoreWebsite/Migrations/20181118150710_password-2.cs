using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class password2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedUntil",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PasswordMask",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondPassword",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordMask",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecondPassword",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedUntil",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
