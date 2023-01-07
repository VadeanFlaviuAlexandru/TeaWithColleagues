using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOC.Migrations
{
    /// <inheritdoc />
    public partial class RestructureAvailabiliy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Availabilities");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Availabilities",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "IdAvailability",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdParticipant",
                table: "Availabilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Availabilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities",
                column: "IdAvailability");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "IdAvailability",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "IdParticipant",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Availabilities");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Availabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities",
                column: "Id");
        }
    }
}
