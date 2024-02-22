using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class CreatorAndContributorNestedViewOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcreatoridentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcreatoraffiliation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcontributoridentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcreatoridentifier");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcreatoraffiliation");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcontributoridentifier");
        }
    }
}
