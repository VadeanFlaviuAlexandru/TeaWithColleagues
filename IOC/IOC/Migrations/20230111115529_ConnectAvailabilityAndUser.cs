using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOC.Migrations
{
    /// <inheritdoc />
    public partial class ConnectAvailabilityAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_User_IdUser",
                table: "Availabilities",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IDUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_User_IdUser",
                table: "Availabilities");

        }
    }
}
