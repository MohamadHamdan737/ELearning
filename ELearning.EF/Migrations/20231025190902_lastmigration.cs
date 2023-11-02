using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.EF.Migrations
{
    /// <inheritdoc />
    public partial class lastmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Favorites",
                newName: "ImageFileName");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileDataImage",
                table: "Favorites",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDataImage",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Favorites",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
