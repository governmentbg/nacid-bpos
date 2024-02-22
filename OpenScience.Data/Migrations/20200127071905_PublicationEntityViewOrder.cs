using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class PublicationEntityViewOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationtitle",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationsubject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationsource",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationsize",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationrelatedidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationpublisher",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationorigindescription",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationlanguage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationfundingreference",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationformat",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationfilelocation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationdescription",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationdate",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcreator",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcoverage",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationcontributor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationaudience",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vieworder",
                table: "publicationalternateidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "arecommonclassificationsvisible",
                table: "institution",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "sets",
                table: "classification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationtitle");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationsubject");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationsource");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationsize");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationrelatedidentifier");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationpublisher");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationorigindescription");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationlanguage");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationfundingreference");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationformat");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationfilelocation");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationdescription");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationdate");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcreator");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcoverage");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationcontributor");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationaudience");

            migrationBuilder.DropColumn(
                name: "vieworder",
                table: "publicationalternateidentifier");

            migrationBuilder.DropColumn(
                name: "arecommonclassificationsvisible",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "sets",
                table: "classification");
        }
    }
}
