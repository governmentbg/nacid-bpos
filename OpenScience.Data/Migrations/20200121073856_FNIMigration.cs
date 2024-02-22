using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OpenScience.Data.Migrations
{
    public partial class FNIMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorinstitution");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropColumn(
                name: "language",
                table: "publicationsubject");

            migrationBuilder.AddColumn<int>(
                name: "languageid",
                table: "publicationsubject",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "moderatorinstitutionid",
                table: "publication",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "licensestartdate",
                table: "publication",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "repositoryurl",
                table: "institution",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "defaultresourcetypeid",
                table: "classification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "metadataformat",
                table: "classification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_publicationsubject_languageid",
                table: "publicationsubject",
                column: "languageid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_moderatorinstitutionid",
                table: "publication",
                column: "moderatorinstitutionid");

            migrationBuilder.CreateIndex(
                name: "IX_classification_defaultidentifiertypeid",
                table: "classification",
                column: "defaultidentifiertypeid");

            migrationBuilder.CreateIndex(
                name: "IX_classification_defaultlicenseconditionid",
                table: "classification",
                column: "defaultlicenseconditionid");

            migrationBuilder.CreateIndex(
                name: "IX_classification_defaultresourcetypeid",
                table: "classification",
                column: "defaultresourcetypeid");

            migrationBuilder.AddForeignKey(
                name: "FK_classification_resourceidentifiertype_defaultidentifiertype~",
                table: "classification",
                column: "defaultidentifiertypeid",
                principalTable: "resourceidentifiertype",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_classification_licensetype_defaultlicenseconditionid",
                table: "classification",
                column: "defaultlicenseconditionid",
                principalTable: "licensetype",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_classification_resourcetype_defaultresourcetypeid",
                table: "classification",
                column: "defaultresourcetypeid",
                principalTable: "resourcetype",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_publication_institution_moderatorinstitutionid",
                table: "publication",
                column: "moderatorinstitutionid",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_publicationsubject_language_languageid",
                table: "publicationsubject",
                column: "languageid",
                principalTable: "language",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classification_resourceidentifiertype_defaultidentifiertype~",
                table: "classification");

            migrationBuilder.DropForeignKey(
                name: "FK_classification_licensetype_defaultlicenseconditionid",
                table: "classification");

            migrationBuilder.DropForeignKey(
                name: "FK_classification_resourcetype_defaultresourcetypeid",
                table: "classification");

            migrationBuilder.DropForeignKey(
                name: "FK_publication_institution_moderatorinstitutionid",
                table: "publication");

            migrationBuilder.DropForeignKey(
                name: "FK_publicationsubject_language_languageid",
                table: "publicationsubject");

            migrationBuilder.DropIndex(
                name: "IX_publicationsubject_languageid",
                table: "publicationsubject");

            migrationBuilder.DropIndex(
                name: "IX_publication_moderatorinstitutionid",
                table: "publication");

            migrationBuilder.DropIndex(
                name: "IX_classification_defaultidentifiertypeid",
                table: "classification");

            migrationBuilder.DropIndex(
                name: "IX_classification_defaultlicenseconditionid",
                table: "classification");

            migrationBuilder.DropIndex(
                name: "IX_classification_defaultresourcetypeid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "languageid",
                table: "publicationsubject");

            migrationBuilder.DropColumn(
                name: "repositoryurl",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "defaultresourcetypeid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "metadataformat",
                table: "classification");

            migrationBuilder.AddColumn<string>(
                name: "language",
                table: "publicationsubject",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "moderatorinstitutionid",
                table: "publication",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "licensestartdate",
                table: "publication",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    firstname = table.Column<string>(nullable: true),
                    firstnamelatin = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    identifierschemeid = table.Column<int>(nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    lastname = table.Column<string>(nullable: true),
                    lastnamelatin = table.Column<string>(nullable: true),
                    latinnamesearchfield = table.Column<string>(nullable: true),
                    mail = table.Column<string>(nullable: true),
                    middlename = table.Column<string>(nullable: true),
                    middlenamelatin = table.Column<string>(nullable: true),
                    namesearchfield = table.Column<string>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.id);
                    table.ForeignKey(
                        name: "FK_author_nameidentifierscheme_identifierschemeid",
                        column: x => x.identifierschemeid,
                        principalTable: "nameidentifierscheme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "authorinstitution",
                columns: table => new
                {
                    authorid = table.Column<int>(nullable: false),
                    institutionid = table.Column<int>(nullable: false),
                    appointmentdate = table.Column<DateTime>(nullable: true),
                    duedate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorinstitution", x => new { x.authorid, x.institutionid });
                    table.ForeignKey(
                        name: "FK_authorinstitution_author_authorid",
                        column: x => x.authorid,
                        principalTable: "author",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_authorinstitution_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_author_identifierschemeid",
                table: "author",
                column: "identifierschemeid");

            migrationBuilder.CreateIndex(
                name: "IX_authorinstitution_institutionid",
                table: "authorinstitution",
                column: "institutionid");
        }
    }
}
