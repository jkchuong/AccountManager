using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountData.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondaryColourP",
                table: "Themes",
                newName: "SecondaryColour");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondaryColour",
                table: "Themes",
                newName: "SecondaryColourP");
        }
    }
}
