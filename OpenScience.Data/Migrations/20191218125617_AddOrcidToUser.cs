using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class AddOrcidToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "orcid",
                table: "user",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orcid",
                table: "user");
        }
    }
}
