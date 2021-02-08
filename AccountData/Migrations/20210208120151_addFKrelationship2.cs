using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountData.Migrations
{
    public partial class addFKrelationship2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_ThemeId",
                table: "Users",
                column: "ThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Themes_ThemeId",
                table: "Users",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Themes_ThemeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ThemeId",
                table: "Users");
        }
    }
}
