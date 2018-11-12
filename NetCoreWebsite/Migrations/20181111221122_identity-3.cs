using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class identity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessage_Messages_MessageId",
                table: "UserMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMessage_AspNetUsers_UserId",
                table: "UserMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage");

            migrationBuilder.RenameTable(
                name: "UserMessage",
                newName: "UserMessages");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessage_MessageId",
                table: "UserMessages",
                newName: "IX_UserMessages_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages",
                columns: new[] { "UserId", "MessageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_Messages_MessageId",
                table: "UserMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_Messages_MessageId",
                table: "UserMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_UserId",
                table: "UserMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages");

            migrationBuilder.RenameTable(
                name: "UserMessages",
                newName: "UserMessage");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessages_MessageId",
                table: "UserMessage",
                newName: "IX_UserMessage_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage",
                columns: new[] { "UserId", "MessageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessage_Messages_MessageId",
                table: "UserMessage",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessage_AspNetUsers_UserId",
                table: "UserMessage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
