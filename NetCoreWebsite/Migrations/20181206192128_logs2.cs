﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreWebsite.Migrations
{
    public partial class logs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Step",
                table: "UserLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "UserLogs");
        }
    }
}