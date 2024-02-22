using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OpenScience.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accessright",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true),
                    uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessright", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audiencetype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audiencetype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classification",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    parentid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classification", x => x.id);
                    table.ForeignKey(
                        name: "FK_classification_classification_parentid",
                        column: x => x.parentid,
                        principalTable: "classification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contributortype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contributortype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "emailtype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "institution",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    isactive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "licensetype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true),
                    uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_licensetype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nameidentifierscheme",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nameidentifierscheme", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organizationalidentifierscheme",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true),
                    uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizationalidentifierscheme", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "relationtype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relationtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resourceidentifiertype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resourceidentifiertype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resourcetype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true),
                    uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resourcetype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resourcetypegeneral",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resourcetypegeneral", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "titletype",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    vieworder = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titletype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userclassification",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false),
                    classificationid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userclassification", x => new { x.userid, x.classificationid });
                });

            migrationBuilder.CreateTable(
                name: "classificationclosure",
                columns: table => new
                {
                    childid = table.Column<int>(nullable: false),
                    parentid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classificationclosure", x => new { x.childid, x.parentid });
                    table.ForeignKey(
                        name: "FK_classificationclosure_classification_childid",
                        column: x => x.childid,
                        principalTable: "classification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_classificationclosure_classification_parentid",
                        column: x => x.parentid,
                        principalTable: "classification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "email",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    typeid = table.Column<int>(nullable: false),
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email", x => x.id);
                    table.ForeignKey(
                        name: "FK_email_emailtype_typeid",
                        column: x => x.typeid,
                        principalTable: "emailtype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    firstname = table.Column<string>(nullable: true),
                    firstnamelatin = table.Column<string>(nullable: true),
                    middlename = table.Column<string>(nullable: true),
                    middlenamelatin = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    lastnamelatin = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    identifierschemeid = table.Column<int>(nullable: false),
                    mail = table.Column<string>(nullable: true),
                    namesearchfield = table.Column<string>(nullable: true),
                    latinnamesearchfield = table.Column<string>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    isactive = table.Column<bool>(nullable: false)
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
                name: "rolepermission",
                columns: table => new
                {
                    roleid = table.Column<int>(nullable: false),
                    permissionid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolepermission", x => new { x.roleid, x.permissionid });
                    table.ForeignKey(
                        name: "FK_rolepermission_role_roleid",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    passwordhash = table.Column<string>(nullable: true),
                    passwordsalt = table.Column<string>(nullable: true),
                    fullname = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    institutionid = table.Column<int>(nullable: false),
                    roleid = table.Column<int>(nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    islocked = table.Column<bool>(nullable: false),
                    createdate = table.Column<DateTime>(nullable: true),
                    updatedate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_institution_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_roleid",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false),
                    roleid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userrole", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_userrole_role_roleid",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "emailaddressee",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    emailid = table.Column<int>(nullable: false),
                    addresseetype = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    sentdate = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailaddressee", x => x.id);
                    table.ForeignKey(
                        name: "FK_emailaddressee_email_emailid",
                        column: x => x.emailid,
                        principalTable: "email",
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

            migrationBuilder.CreateTable(
                name: "passwordtoken",
                columns: table => new
                {
                    value = table.Column<string>(nullable: false),
                    expirationtime = table.Column<DateTime>(nullable: false),
                    isused = table.Column<bool>(nullable: false),
                    userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passwordtoken", x => x.value);
                    table.ForeignKey(
                        name: "FK_passwordtoken_user_userid",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publication",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    resourcetypeid = table.Column<int>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    identifiertypeid = table.Column<int>(nullable: true),
                    publishyear = table.Column<int>(nullable: false),
                    publishmonth = table.Column<int>(nullable: true),
                    publishday = table.Column<int>(nullable: true),
                    resourceversion = table.Column<string>(nullable: true),
                    resourceversionuri = table.Column<string>(nullable: true),
                    citationtitle = table.Column<string>(nullable: true),
                    citationvolume = table.Column<int>(nullable: true),
                    citationissue = table.Column<int>(nullable: true),
                    citationstartpage = table.Column<int>(nullable: true),
                    citationendpage = table.Column<int>(nullable: true),
                    citationedition = table.Column<int>(nullable: true),
                    citationconferenceplace = table.Column<string>(nullable: true),
                    citationconferencestartdate = table.Column<DateTime>(nullable: true),
                    citationconferenceenddate = table.Column<DateTime>(nullable: true),
                    accessrightid = table.Column<int>(nullable: false),
                    embargoperiodstart = table.Column<DateTime>(nullable: true),
                    embargoperiodend = table.Column<DateTime>(nullable: true),
                    licensetypeid = table.Column<int>(nullable: true),
                    otherlicensecondition = table.Column<string>(nullable: true),
                    otherlicenseurl = table.Column<string>(nullable: true),
                    licensestartdate = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    modificationdate = table.Column<DateTime>(nullable: false),
                    createdbyuserid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publication", x => x.id);
                    table.ForeignKey(
                        name: "FK_publication_accessright_accessrightid",
                        column: x => x.accessrightid,
                        principalTable: "accessright",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publication_user_createdbyuserid",
                        column: x => x.createdbyuserid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publication_resourceidentifiertype_identifiertypeid",
                        column: x => x.identifiertypeid,
                        principalTable: "resourceidentifiertype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publication_licensetype_licensetypeid",
                        column: x => x.licensetypeid,
                        principalTable: "licensetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publication_resourcetype_resourcetypeid",
                        column: x => x.resourcetypeid,
                        principalTable: "resourcetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationalternateidentifier",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    typeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationalternateidentifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationalternateidentifier_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationalternateidentifier_resourceidentifiertype_typeid",
                        column: x => x.typeid,
                        principalTable: "resourceidentifiertype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationaudience",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    typeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationaudience", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationaudience_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationaudience_audiencetype_typeid",
                        column: x => x.typeid,
                        principalTable: "audiencetype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationclassification",
                columns: table => new
                {
                    publicationid = table.Column<int>(nullable: false),
                    classificationid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationclassification", x => new { x.publicationid, x.classificationid });
                    table.ForeignKey(
                        name: "FK_publicationclassification_classification_classificationid",
                        column: x => x.classificationid,
                        principalTable: "classification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationclassification_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcontributor",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    typeid = table.Column<int>(nullable: true),
                    nametype = table.Column<int>(nullable: false),
                    firstname = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    institutionaffiliationname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcontributor", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcontributor_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationcontributor_contributortype_typeid",
                        column: x => x.typeid,
                        principalTable: "contributortype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcoverage",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcoverage", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcoverage_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcreator",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    firstname = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    nametype = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcreator", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcreator_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationdate",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationdate", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationdate_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationdescription",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    languageid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationdescription", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationdescription_language_languageid",
                        column: x => x.languageid,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationdescription_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationfile",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key = table.Column<Guid>(nullable: false),
                    hash = table.Column<string>(nullable: true),
                    size = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    mimetype = table.Column<string>(nullable: true),
                    dbid = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationfile_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationformat",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationformat", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationformat_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationfundingreference",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    schemeid = table.Column<int>(nullable: true),
                    awardnumber = table.Column<string>(nullable: true),
                    awarduri = table.Column<string>(nullable: true),
                    awardtitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationfundingreference", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationfundingreference_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationfundingreference_organizationalidentifierscheme_~",
                        column: x => x.schemeid,
                        principalTable: "organizationalidentifierscheme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationlanguage",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    languageid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationlanguage", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationlanguage_language_languageid",
                        column: x => x.languageid,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationlanguage_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationorigindescription",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    baseurl = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true),
                    datestamp = table.Column<string>(nullable: true),
                    metadatanamespace = table.Column<string>(nullable: true),
                    origindescription = table.Column<string>(nullable: true),
                    harvestdate = table.Column<DateTime>(nullable: false),
                    altered = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationorigindescription", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationorigindescription_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationpublisher",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationpublisher", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationpublisher_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationrelatedidentifier",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    typeid = table.Column<int>(nullable: false),
                    relationtypeid = table.Column<int>(nullable: false),
                    relatedmetadatascheme = table.Column<string>(nullable: true),
                    schemeuri = table.Column<string>(nullable: true),
                    schemetype = table.Column<string>(nullable: true),
                    resourcetypegeneralid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationrelatedidentifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationrelatedidentifier_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationrelatedidentifier_relationtype_relationtypeid",
                        column: x => x.relationtypeid,
                        principalTable: "relationtype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationrelatedidentifier_resourcetypegeneral_resourcety~",
                        column: x => x.resourcetypegeneralid,
                        principalTable: "resourcetypegeneral",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationrelatedidentifier_resourceidentifiertype_typeid",
                        column: x => x.typeid,
                        principalTable: "resourceidentifiertype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationsize",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationsize", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationsize_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationsource",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationsource", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationsource_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationsubject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationsubject", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationsubject_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationtitle",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    typeid = table.Column<int>(nullable: true),
                    languageid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationtitle", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationtitle_language_languageid",
                        column: x => x.languageid,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationtitle_publication_publicationid",
                        column: x => x.publicationid,
                        principalTable: "publication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationtitle_titletype_typeid",
                        column: x => x.typeid,
                        principalTable: "titletype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcontributoridentifier",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationcontributorid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    schemeid = table.Column<int>(nullable: true),
                    organizationalschemeid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcontributoridentifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcontributoridentifier_organizationalidentifiersc~",
                        column: x => x.organizationalschemeid,
                        principalTable: "organizationalidentifierscheme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationcontributoridentifier_publicationcontributor_pub~",
                        column: x => x.publicationcontributorid,
                        principalTable: "publicationcontributor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationcontributoridentifier_nameidentifierscheme_schem~",
                        column: x => x.schemeid,
                        principalTable: "nameidentifierscheme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcreatoraffiliation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationcreatorid = table.Column<int>(nullable: false),
                    institutionname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcreatoraffiliation", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcreatoraffiliation_publicationcreator_publicatio~",
                        column: x => x.publicationcreatorid,
                        principalTable: "publicationcreator",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publicationcreatoridentifier",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    version = table.Column<int>(nullable: false),
                    publicationcreatorid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    schemeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicationcreatoridentifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_publicationcreatoridentifier_publicationcreator_publication~",
                        column: x => x.publicationcreatorid,
                        principalTable: "publicationcreator",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publicationcreatoridentifier_nameidentifierscheme_schemeid",
                        column: x => x.schemeid,
                        principalTable: "nameidentifierscheme",
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

            migrationBuilder.CreateIndex(
                name: "IX_classification_parentid",
                table: "classification",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_classificationclosure_parentid",
                table: "classificationclosure",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_email_typeid",
                table: "email",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_emailaddressee_emailid",
                table: "emailaddressee",
                column: "emailid");

            migrationBuilder.CreateIndex(
                name: "IX_passwordtoken_userid",
                table: "passwordtoken",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_accessrightid",
                table: "publication",
                column: "accessrightid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_createdbyuserid",
                table: "publication",
                column: "createdbyuserid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_identifiertypeid",
                table: "publication",
                column: "identifiertypeid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_licensetypeid",
                table: "publication",
                column: "licensetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_publication_resourcetypeid",
                table: "publication",
                column: "resourcetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationalternateidentifier_publicationid",
                table: "publicationalternateidentifier",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationalternateidentifier_typeid",
                table: "publicationalternateidentifier",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationaudience_publicationid",
                table: "publicationaudience",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationaudience_typeid",
                table: "publicationaudience",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationclassification_classificationid",
                table: "publicationclassification",
                column: "classificationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcontributor_publicationid",
                table: "publicationcontributor",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcontributor_typeid",
                table: "publicationcontributor",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcontributoridentifier_organizationalschemeid",
                table: "publicationcontributoridentifier",
                column: "organizationalschemeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcontributoridentifier_publicationcontributorid",
                table: "publicationcontributoridentifier",
                column: "publicationcontributorid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcontributoridentifier_schemeid",
                table: "publicationcontributoridentifier",
                column: "schemeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcoverage_publicationid",
                table: "publicationcoverage",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcreator_publicationid",
                table: "publicationcreator",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcreatoraffiliation_publicationcreatorid",
                table: "publicationcreatoraffiliation",
                column: "publicationcreatorid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcreatoridentifier_publicationcreatorid",
                table: "publicationcreatoridentifier",
                column: "publicationcreatorid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationcreatoridentifier_schemeid",
                table: "publicationcreatoridentifier",
                column: "schemeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationdate_publicationid",
                table: "publicationdate",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationdescription_languageid",
                table: "publicationdescription",
                column: "languageid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationdescription_publicationid",
                table: "publicationdescription",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationfile_publicationid",
                table: "publicationfile",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationformat_publicationid",
                table: "publicationformat",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationfundingreference_publicationid",
                table: "publicationfundingreference",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationfundingreference_schemeid",
                table: "publicationfundingreference",
                column: "schemeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationlanguage_languageid",
                table: "publicationlanguage",
                column: "languageid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationlanguage_publicationid",
                table: "publicationlanguage",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationorigindescription_publicationid",
                table: "publicationorigindescription",
                column: "publicationid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_publicationpublisher_publicationid",
                table: "publicationpublisher",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationrelatedidentifier_publicationid",
                table: "publicationrelatedidentifier",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationrelatedidentifier_relationtypeid",
                table: "publicationrelatedidentifier",
                column: "relationtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationrelatedidentifier_resourcetypegeneralid",
                table: "publicationrelatedidentifier",
                column: "resourcetypegeneralid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationrelatedidentifier_typeid",
                table: "publicationrelatedidentifier",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationsize_publicationid",
                table: "publicationsize",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationsource_publicationid",
                table: "publicationsource",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationsubject_publicationid",
                table: "publicationsubject",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationtitle_languageid",
                table: "publicationtitle",
                column: "languageid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationtitle_publicationid",
                table: "publicationtitle",
                column: "publicationid");

            migrationBuilder.CreateIndex(
                name: "IX_publicationtitle_typeid",
                table: "publicationtitle",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_user_institutionid",
                table: "user",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_user_roleid",
                table: "user",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_userrole_roleid",
                table: "userrole",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorinstitution");

            migrationBuilder.DropTable(
                name: "classificationclosure");

            migrationBuilder.DropTable(
                name: "emailaddressee");

            migrationBuilder.DropTable(
                name: "passwordtoken");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "publicationalternateidentifier");

            migrationBuilder.DropTable(
                name: "publicationaudience");

            migrationBuilder.DropTable(
                name: "publicationclassification");

            migrationBuilder.DropTable(
                name: "publicationcontributoridentifier");

            migrationBuilder.DropTable(
                name: "publicationcoverage");

            migrationBuilder.DropTable(
                name: "publicationcreatoraffiliation");

            migrationBuilder.DropTable(
                name: "publicationcreatoridentifier");

            migrationBuilder.DropTable(
                name: "publicationdate");

            migrationBuilder.DropTable(
                name: "publicationdescription");

            migrationBuilder.DropTable(
                name: "publicationfile");

            migrationBuilder.DropTable(
                name: "publicationformat");

            migrationBuilder.DropTable(
                name: "publicationfundingreference");

            migrationBuilder.DropTable(
                name: "publicationlanguage");

            migrationBuilder.DropTable(
                name: "publicationorigindescription");

            migrationBuilder.DropTable(
                name: "publicationpublisher");

            migrationBuilder.DropTable(
                name: "publicationrelatedidentifier");

            migrationBuilder.DropTable(
                name: "publicationsize");

            migrationBuilder.DropTable(
                name: "publicationsource");

            migrationBuilder.DropTable(
                name: "publicationsubject");

            migrationBuilder.DropTable(
                name: "publicationtitle");

            migrationBuilder.DropTable(
                name: "rolepermission");

            migrationBuilder.DropTable(
                name: "userclassification");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "email");

            migrationBuilder.DropTable(
                name: "audiencetype");

            migrationBuilder.DropTable(
                name: "classification");

            migrationBuilder.DropTable(
                name: "publicationcontributor");

            migrationBuilder.DropTable(
                name: "publicationcreator");

            migrationBuilder.DropTable(
                name: "organizationalidentifierscheme");

            migrationBuilder.DropTable(
                name: "relationtype");

            migrationBuilder.DropTable(
                name: "resourcetypegeneral");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropTable(
                name: "titletype");

            migrationBuilder.DropTable(
                name: "nameidentifierscheme");

            migrationBuilder.DropTable(
                name: "emailtype");

            migrationBuilder.DropTable(
                name: "contributortype");

            migrationBuilder.DropTable(
                name: "publication");

            migrationBuilder.DropTable(
                name: "accessright");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "resourceidentifiertype");

            migrationBuilder.DropTable(
                name: "licensetype");

            migrationBuilder.DropTable(
                name: "resourcetype");

            migrationBuilder.DropTable(
                name: "institution");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
