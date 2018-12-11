using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class users13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordMask",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "SecondPasswordId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondPasswordId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PasswordMask",
                table: "Users",
                nullable: true);
        }
    }
}
