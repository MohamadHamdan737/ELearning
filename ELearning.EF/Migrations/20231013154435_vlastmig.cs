using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.EF.Migrations
{
    /// <inheritdoc />
    public partial class vlastmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoDescription",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "VideoTitle",
                table: "courses",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "VideoContent",
                table: "courses",
                newName: "FileData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "courses",
                newName: "VideoTitle");

            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "courses",
                newName: "VideoContent");

            migrationBuilder.AddColumn<string>(
                name: "VideoDescription",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
