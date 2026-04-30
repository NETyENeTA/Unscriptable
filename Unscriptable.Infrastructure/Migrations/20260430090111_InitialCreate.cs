using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Unscriptable.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "directions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_directions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    isrepeatable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    publishedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    durationminutes = table.Column<int>(type: "integer", nullable: true),
                    ispublic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    passingscore = table.Column<int>(type: "integer", nullable: true),
                    maxattempts = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: true),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    directionid = table.Column<int>(type: "integer", nullable: false),
                    courseid = table.Column<int>(type: "integer", nullable: false),
                    projectid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_groups_courses_courseid",
                        column: x => x.courseid,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_groups_directions_directionid",
                        column: x => x.directionid,
                        principalTable: "directions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_groups_projects_projectid",
                        column: x => x.projectid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    answertype = table.Column<string>(type: "text", nullable: false),
                    isscoring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    maxscore = table.Column<int>(type: "integer", nullable: true),
                    testid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_questions_tests_testid",
                        column: x => x.testid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_courses",
                columns: table => new
                {
                    coursesid = table.Column<int>(type: "integer", nullable: false),
                    testsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_courses", x => new { x.coursesid, x.testsid });
                    table.ForeignKey(
                        name: "fk_test_courses_courses_coursesid",
                        column: x => x.coursesid,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_courses_tests_testsid",
                        column: x => x.testsid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_directions",
                columns: table => new
                {
                    directionsid = table.Column<int>(type: "integer", nullable: false),
                    testsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_directions", x => new { x.directionsid, x.testsid });
                    table.ForeignKey(
                        name: "fk_test_directions_directions_directionsid",
                        column: x => x.directionsid,
                        principalTable: "directions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_directions_tests_testsid",
                        column: x => x.testsid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_projects",
                columns: table => new
                {
                    projectsid = table.Column<int>(type: "integer", nullable: false),
                    testsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_projects", x => new { x.projectsid, x.testsid });
                    table.ForeignKey(
                        name: "fk_test_projects_projects_projectsid",
                        column: x => x.projectsid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_projects_tests_testsid",
                        column: x => x.testsid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refreshtokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tokenhash = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiresat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revokedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refreshtokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refreshtokens_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    vkprofilelink = table.Column<string>(type: "text", nullable: false),
                    avatarpath = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_groups",
                columns: table => new
                {
                    groupsid = table.Column<int>(type: "integer", nullable: false),
                    testsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_groups", x => new { x.groupsid, x.testsid });
                    table.ForeignKey(
                        name: "fk_test_groups_groups_groupsid",
                        column: x => x.groupsid,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_groups_tests_testsid",
                        column: x => x.testsid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    iscorrect = table.Column<bool>(type: "boolean", nullable: false),
                    questionid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_answers_questions_questionid",
                        column: x => x.questionid,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attempts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    submittedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    testid = table.Column<int>(type: "integer", nullable: false),
                    studentid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attempts", x => x.id);
                    table.ForeignKey(
                        name: "fk_attempts_students_studentid",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attempts_tests_testid",
                        column: x => x.testid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_groups",
                columns: table => new
                {
                    groupsid = table.Column<int>(type: "integer", nullable: false),
                    studentsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_groups", x => new { x.groupsid, x.studentsid });
                    table.ForeignKey(
                        name: "fk_student_groups_groups_groupsid",
                        column: x => x.groupsid,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_groups_students_studentsid",
                        column: x => x.studentsid,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_students",
                columns: table => new
                {
                    studentsid = table.Column<int>(type: "integer", nullable: false),
                    testsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_students", x => new { x.studentsid, x.testsid });
                    table.ForeignKey(
                        name: "fk_test_students_students_studentsid",
                        column: x => x.studentsid,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_test_students_tests_testsid",
                        column: x => x.testsid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "testresults",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    passed = table.Column<bool>(type: "boolean", nullable: false),
                    testid = table.Column<int>(type: "integer", nullable: false),
                    attemptid = table.Column<int>(type: "integer", nullable: false),
                    studentid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_testresults", x => x.id);
                    table.ForeignKey(
                        name: "fk_testresults_attempts_attemptid",
                        column: x => x.attemptid,
                        principalTable: "attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_testresults_students_studentid",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_testresults_tests_testid",
                        column: x => x.testid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userattemptanswers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iscorrect = table.Column<bool>(type: "boolean", nullable: false),
                    scoreawarded = table.Column<int>(type: "integer", nullable: false),
                    attemptid = table.Column<int>(type: "integer", nullable: false),
                    questionid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userattemptanswers", x => x.id);
                    table.ForeignKey(
                        name: "fk_userattemptanswers_attempts_attemptid",
                        column: x => x.attemptid,
                        principalTable: "attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_userattemptanswers_questions_questionid",
                        column: x => x.questionid,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userselectedoptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userattemptanswerid = table.Column<int>(type: "integer", nullable: false),
                    answerid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userselectedoptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_userselectedoptions_answers_answerid",
                        column: x => x.answerid,
                        principalTable: "answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_userselectedoptions_userattemptanswers_userattemptanswerid",
                        column: x => x.userattemptanswerid,
                        principalTable: "userattemptanswers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usertextanswers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    textanswer = table.Column<string>(type: "text", nullable: false),
                    userattemptanswerid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usertextanswers", x => x.id);
                    table.ForeignKey(
                        name: "fk_usertextanswers_userattemptanswers_userattemptanswerid",
                        column: x => x.userattemptanswerid,
                        principalTable: "userattemptanswers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_answers_questionid",
                table: "answers",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "ix_attempts_studentid",
                table: "attempts",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "ix_attempts_testid",
                table: "attempts",
                column: "testid");

            migrationBuilder.CreateIndex(
                name: "ix_courses_name",
                table: "courses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_directions_name",
                table: "directions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_groups_courseid",
                table: "groups",
                column: "courseid");

            migrationBuilder.CreateIndex(
                name: "ix_groups_directionid",
                table: "groups",
                column: "directionid");

            migrationBuilder.CreateIndex(
                name: "ix_groups_name",
                table: "groups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_groups_projectid",
                table: "groups",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "ix_projects_name",
                table: "projects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_questions_testid_number",
                table: "questions",
                columns: new[] { "testid", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refreshtokens_tokenhash",
                table: "refreshtokens",
                column: "tokenhash");

            migrationBuilder.CreateIndex(
                name: "ix_refreshtokens_userid",
                table: "refreshtokens",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_student_groups_studentsid",
                table: "student_groups",
                column: "studentsid");

            migrationBuilder.CreateIndex(
                name: "ix_students_userid",
                table: "students",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_test_courses_testsid",
                table: "test_courses",
                column: "testsid");

            migrationBuilder.CreateIndex(
                name: "ix_test_directions_testsid",
                table: "test_directions",
                column: "testsid");

            migrationBuilder.CreateIndex(
                name: "ix_test_groups_testsid",
                table: "test_groups",
                column: "testsid");

            migrationBuilder.CreateIndex(
                name: "ix_test_projects_testsid",
                table: "test_projects",
                column: "testsid");

            migrationBuilder.CreateIndex(
                name: "ix_test_students_testsid",
                table: "test_students",
                column: "testsid");

            migrationBuilder.CreateIndex(
                name: "ix_testresults_attemptid",
                table: "testresults",
                column: "attemptid");

            migrationBuilder.CreateIndex(
                name: "ix_testresults_studentid",
                table: "testresults",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "ix_testresults_testid_studentid_attemptid",
                table: "testresults",
                columns: new[] { "testid", "studentid", "attemptid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userattemptanswers_attemptid_questionid",
                table: "userattemptanswers",
                columns: new[] { "attemptid", "questionid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userattemptanswers_questionid",
                table: "userattemptanswers",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userselectedoptions_answerid",
                table: "userselectedoptions",
                column: "answerid");

            migrationBuilder.CreateIndex(
                name: "ix_userselectedoptions_userattemptanswerid",
                table: "userselectedoptions",
                column: "userattemptanswerid");

            migrationBuilder.CreateIndex(
                name: "ix_usertextanswers_userattemptanswerid",
                table: "usertextanswers",
                column: "userattemptanswerid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refreshtokens");

            migrationBuilder.DropTable(
                name: "student_groups");

            migrationBuilder.DropTable(
                name: "test_courses");

            migrationBuilder.DropTable(
                name: "test_directions");

            migrationBuilder.DropTable(
                name: "test_groups");

            migrationBuilder.DropTable(
                name: "test_projects");

            migrationBuilder.DropTable(
                name: "test_students");

            migrationBuilder.DropTable(
                name: "testresults");

            migrationBuilder.DropTable(
                name: "userselectedoptions");

            migrationBuilder.DropTable(
                name: "usertextanswers");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "userattemptanswers");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "directions");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "attempts");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
