using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountData.Migrations
{
    public partial class AddedAgressivenessColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AggressiveOn",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggressiveOn",
                table: "Users");
        }
    }
}
