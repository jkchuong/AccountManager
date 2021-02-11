using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountData.Migrations
{
    public partial class AddSaveExistColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SaveExist",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaveExist",
                table: "Users");
        }
    }
}
