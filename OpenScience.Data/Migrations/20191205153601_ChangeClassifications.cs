using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OpenScience.Data.Migrations
{
    public partial class ChangeClassifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "defaultlicensestartdate",
                table: "classification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "harevesturl",
                table: "classification",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "publicationfilelocation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    accessrightsuri = table.Column<string>(nullable: true),
                    mimetype = table.Column<string>(nullable: true),
                    objecttype = table.Column<int>(nullable: false),
                    fileurl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationfilelocation", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationfilelocation_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classification_organizationid",
                table: "classification",
                column: "organizationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationfilelocation_publicationid",
                table: "publicationfilelocation",
                column: "publicationid");

            migrationBuilder.AddForeignKey(
                name: "FK_classification_institution_organizationid",
                table: "classification",
                column: "organizationid",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classification_institution_organizationid",
                table: "classification");

            migrationBuilder.DropTable(
                name: "publicationfilelocation");

            migrationBuilder.DropIndex(
                name: "IX_classification_organizationid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "defaultlicensestartdate",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "harevesturl",
                table: "classification");
        }
    }
}
