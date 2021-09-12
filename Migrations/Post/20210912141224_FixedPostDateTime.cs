using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OOP_WORKSHOP_PROJECT.Migrations.Post
{
    public partial class FixedPostDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "Posts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "Posts",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "Posts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
