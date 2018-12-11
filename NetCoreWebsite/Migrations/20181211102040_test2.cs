using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "NotFoundUsers");

            migrationBuilder.AddColumn<string>(
                name: "Mask",
                table: "NotFoundUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mask",
                table: "NotFoundUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "NotFoundUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
