using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class user10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondPassword",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserSecondPassword",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userFK = table.Column<int>(nullable: true),
                    Mask = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecondPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecondPassword_Users_userFK",
                        column: x => x.userFK,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondPassword_userFK",
                table: "UserSecondPassword",
                column: "userFK",
                unique: true,
                filter: "[userFK] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecondPassword");

            migrationBuilder.AddColumn<string>(
                name: "SecondPassword",
                table: "Users",
                nullable: true);
        }
    }
}
