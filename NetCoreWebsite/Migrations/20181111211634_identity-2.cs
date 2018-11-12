using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class identity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_MaxFailedCount",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_MaxFailedCount",
                table: "AspNetUsers",
                column: "MaxFailedCount");
        }
    }
}
