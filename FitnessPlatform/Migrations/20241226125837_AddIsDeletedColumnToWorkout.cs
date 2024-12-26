﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumnToWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Workouts");
        }
    }
}
