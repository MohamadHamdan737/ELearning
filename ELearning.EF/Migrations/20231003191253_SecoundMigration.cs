﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.EF.Migrations
{
    /// <inheritdoc />
    public partial class SecoundMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Majoring",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Majoring",
                table: "Instructors");
        }
    }
}
