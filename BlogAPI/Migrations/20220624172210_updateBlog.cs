using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    public partial class updateBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document",
                table: "Blogs",
                newName: "Entry");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Blogs",
                newName: "PublishDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "Blogs",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Entry",
                table: "Blogs",
                newName: "Document");
        }
    }
}
