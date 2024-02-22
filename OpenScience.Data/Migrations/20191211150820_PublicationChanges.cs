using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class PublicationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "harevesturl",
                table: "classification",
                newName: "harvesturl");

            migrationBuilder.AlterColumn<int>(
                name: "languageid",
                table: "publicationdescription",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "moderatorinstitutionid",
                table: "publication",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "moderatorinstitutionid",
                table: "publication");

            migrationBuilder.RenameColumn(
                name: "harvesturl",
                table: "classification",
                newName: "harevesturl");

            migrationBuilder.AlterColumn<int>(
                name: "languageid",
                table: "publicationdescription",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
