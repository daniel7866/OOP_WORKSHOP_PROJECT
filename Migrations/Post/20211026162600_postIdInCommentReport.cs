using Microsoft.EntityFrameworkCore.Migrations;

namespace OOP_WORKSHOP_PROJECT.Migrations.Post
{
    public partial class postIdInCommentReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentReport_PostId",
                table: "Reports",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentReport_PostId",
                table: "Reports");
        }
    }
}
