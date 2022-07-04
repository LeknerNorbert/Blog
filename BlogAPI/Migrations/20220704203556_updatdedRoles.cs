using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    public partial class updatdedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanUpdateBlog",
                table: "Roles",
                newName: "CanUpdate");

            migrationBuilder.RenameColumn(
                name: "CanReadBlog",
                table: "Roles",
                newName: "CanRead");

            migrationBuilder.RenameColumn(
                name: "CanDeleteBlog",
                table: "Roles",
                newName: "CanDelete");

            migrationBuilder.RenameColumn(
                name: "CanCreateBlog",
                table: "Roles",
                newName: "CanCreate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanUpdate",
                table: "Roles",
                newName: "CanUpdateBlog");

            migrationBuilder.RenameColumn(
                name: "CanRead",
                table: "Roles",
                newName: "CanReadBlog");

            migrationBuilder.RenameColumn(
                name: "CanDelete",
                table: "Roles",
                newName: "CanDeleteBlog");

            migrationBuilder.RenameColumn(
                name: "CanCreate",
                table: "Roles",
                newName: "CanCreateBlog");
        }
    }
}
