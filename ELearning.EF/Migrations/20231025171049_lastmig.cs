using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.EF.Migrations
{
    /// <inheritdoc />
    public partial class lastmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "courses",
                newName: "VideoName");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "courses",
                newName: "VideoFileName");

            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "courses",
                newName: "VideoFileData");

            migrationBuilder.RenameColumn(
                name: "FileCertificate",
                table: "courses",
                newName: "FileDataImage");

            migrationBuilder.AddColumn<string>(
                name: "CertificateFileName",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileDataCertificate",
                table: "courses",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateFileName",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "FileDataCertificate",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "VideoName",
                table: "courses",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "VideoFileName",
                table: "courses",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "VideoFileData",
                table: "courses",
                newName: "FileData");

            migrationBuilder.RenameColumn(
                name: "FileDataImage",
                table: "courses",
                newName: "FileCertificate");
        }
    }
}
