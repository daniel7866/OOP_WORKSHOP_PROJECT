using Microsoft.EntityFrameworkCore.Migrations;

namespace OOP_WORKSHOP_PROJECT.Migrations.Post
{
    public partial class reportMigrationCommentFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "Reports",
                newName: "CommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Reports",
                newName: "ReportId");
        }
    }
}
