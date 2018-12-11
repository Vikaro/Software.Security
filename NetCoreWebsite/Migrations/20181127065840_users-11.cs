using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class users11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSecondPassword_Users_userFK",
                table: "UserSecondPassword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSecondPassword",
                table: "UserSecondPassword");

            migrationBuilder.RenameTable(
                name: "UserSecondPassword",
                newName: "UserSecondPasswords");

            migrationBuilder.RenameIndex(
                name: "IX_UserSecondPassword_userFK",
                table: "UserSecondPasswords",
                newName: "IX_UserSecondPasswords_userFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSecondPasswords",
                table: "UserSecondPasswords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSecondPasswords_Users_userFK",
                table: "UserSecondPasswords",
                column: "userFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSecondPasswords_Users_userFK",
                table: "UserSecondPasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSecondPasswords",
                table: "UserSecondPasswords");

            migrationBuilder.RenameTable(
                name: "UserSecondPasswords",
                newName: "UserSecondPassword");

            migrationBuilder.RenameIndex(
                name: "IX_UserSecondPasswords_userFK",
                table: "UserSecondPassword",
                newName: "IX_UserSecondPassword_userFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSecondPassword",
                table: "UserSecondPassword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSecondPassword_Users_userFK",
                table: "UserSecondPassword",
                column: "userFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
