using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class users12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSecondPasswords_userFK",
                table: "UserSecondPasswords");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondPasswords_userFK",
                table: "UserSecondPasswords",
                column: "userFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSecondPasswords_userFK",
                table: "UserSecondPasswords");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondPasswords_userFK",
                table: "UserSecondPasswords",
                column: "userFK",
                unique: true,
                filter: "[userFK] IS NOT NULL");
        }
    }
}
