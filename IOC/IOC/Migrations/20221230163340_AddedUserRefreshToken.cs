using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOC.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRefreshToken",
                columns: table => new
                {
                    IDUserRefreshToken = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string?>(type: "varchar(50)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime?>(type: "datetime", nullable: true),
                    IDUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshToken", x => x.IDUserRefreshToken);
                    table.ForeignKey(
                        name: "FK_UserRefreshToken_User_IDUser",
                        column: x => x.IDUser,
                        principalTable: "User",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Cascade
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_IDUser",
                table: "UserRefreshToken",
                column: "IDUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshToken");
        }
    }
}
