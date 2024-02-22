using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenScience.Data.Migrations
{
    public partial class UserAndClassificationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_institution_institutionid",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_institutionid",
                table: "user");

            migrationBuilder.DropColumn(
                name: "institutionid",
                table: "user");

            migrationBuilder.AddColumn<int>(
                name: "defaultaccessrightid",
                table: "classification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "defaultidentifiertypeid",
                table: "classification",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "defaultlicenseconditionid",
                table: "classification",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isopenairepropagationenabled",
                table: "classification",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isreadonly",
                table: "classification",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "organizationid",
                table: "classification",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "userinstitution",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false),
                    institutionid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userinstitution", x => new { x.userid, x.institutionid });
                    table.ForeignKey(
                        name: "FK_userinstitution_user_userid",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinstitution");

            migrationBuilder.DropColumn(
                name: "defaultaccessrightid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "defaultidentifiertypeid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "defaultlicenseconditionid",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "isopenairepropagationenabled",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "isreadonly",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "organizationid",
                table: "classification");

            migrationBuilder.AddColumn<int>(
                name: "institutionid",
                table: "user",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_user_institutionid",
                table: "user",
                column: "institutionid");

            migrationBuilder.AddForeignKey(
                name: "FK_user_institution_institutionid",
                table: "user",
                column: "institutionid",
                principalTable: "institution",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
