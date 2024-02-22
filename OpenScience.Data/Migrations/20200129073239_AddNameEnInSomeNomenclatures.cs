using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class AddNameEnInSomeNomenclatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "titletype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "resourcetype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "resourceidentifiertype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "relationtype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "contributortype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameen",
                table: "accessright",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nameen",
                table: "titletype");

            migrationBuilder.DropColumn(
                name: "nameen",
                table: "resourcetype");

            migrationBuilder.DropColumn(
                name: "nameen",
                table: "resourceidentifiertype");

            migrationBuilder.DropColumn(
                name: "nameen",
                table: "relationtype");

            migrationBuilder.DropColumn(
                name: "nameen",
                table: "contributortype");

            migrationBuilder.DropColumn(
                name: "nameen",
                table: "accessright");
        }
    }
}
