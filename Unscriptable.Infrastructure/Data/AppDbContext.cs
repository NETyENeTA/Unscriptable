using Microsoft.EntityFrameworkCore;
using Unscriptable.Domain.Entitties;


namespace Unscriptable.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Attempt> Attempts { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<Testresult> Testresults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userattemptanswer> Userattemptanswers { get; set; }

    public virtual DbSet<Userselectedoption> Userselectedoptions { get; set; }

    public virtual DbSet<Usertextanswer> Usertextanswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=testing_db;Username=postgres;Password=Sigmarule1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_answers");

            entity.ToTable("answers");

            entity.HasIndex(e => e.Questionid, "ix_answers_questionid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iscorrect).HasColumnName("iscorrect");
            entity.Property(e => e.Questionid).HasColumnName("questionid");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Questionid)
                .HasConstraintName("fk_answers_questions_questionid");
        });

        modelBuilder.Entity<Attempt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_attempts");

            entity.ToTable("attempts");

            entity.HasIndex(e => e.Studentid, "ix_attempts_studentid");

            entity.HasIndex(e => e.Testid, "ix_attempts_testid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Startedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("startedat");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
            entity.Property(e => e.Submittedat).HasColumnName("submittedat");
            entity.Property(e => e.Testid).HasColumnName("testid");

            entity.HasOne(d => d.Student).WithMany(p => p.Attempts)
                .HasForeignKey(d => d.Studentid)
                .HasConstraintName("fk_attempts_students_studentid");

            entity.HasOne(d => d.Test).WithMany(p => p.Attempts)
                .HasForeignKey(d => d.Testid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_attempts_tests_testid");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_courses");

            entity.ToTable("courses");

            entity.HasIndex(e => e.Name, "ix_courses_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasMany(d => d.Tests).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "TestCourse",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("Testsid")
                        .HasConstraintName("fk_test_courses_tests_testsid"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("Coursesid")
                        .HasConstraintName("fk_test_courses_courses_coursesid"),
                    j =>
                    {
                        j.HasKey("Coursesid", "Testsid").HasName("pk_test_courses");
                        j.ToTable("test_courses");
                        j.HasIndex(new[] { "Testsid" }, "ix_test_courses_testsid");
                        j.IndexerProperty<int>("Coursesid").HasColumnName("coursesid");
                        j.IndexerProperty<int>("Testsid").HasColumnName("testsid");
                    });
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_directions");

            entity.ToTable("directions");

            entity.HasIndex(e => e.Name, "ix_directions_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasMany(d => d.Tests).WithMany(p => p.Directions)
                .UsingEntity<Dictionary<string, object>>(
                    "TestDirection",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("Testsid")
                        .HasConstraintName("fk_test_directions_tests_testsid"),
                    l => l.HasOne<Direction>().WithMany()
                        .HasForeignKey("Directionsid")
                        .HasConstraintName("fk_test_directions_directions_directionsid"),
                    j =>
                    {
                        j.HasKey("Directionsid", "Testsid").HasName("pk_test_directions");
                        j.ToTable("test_directions");
                        j.HasIndex(new[] { "Testsid" }, "ix_test_directions_testsid");
                        j.IndexerProperty<int>("Directionsid").HasColumnName("directionsid");
                        j.IndexerProperty<int>("Testsid").HasColumnName("testsid");
                    });
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_groups");

            entity.ToTable("groups");

            entity.HasIndex(e => e.Courseid, "ix_groups_courseid");

            entity.HasIndex(e => e.Directionid, "ix_groups_directionid");

            entity.HasIndex(e => e.Name, "ix_groups_name").IsUnique();

            entity.HasIndex(e => e.Projectid, "ix_groups_projectid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Directionid).HasColumnName("directionid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Projectid).HasColumnName("projectid");

            entity.HasOne(d => d.Course).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Courseid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_groups_courses_courseid");

            entity.HasOne(d => d.Direction).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Directionid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_groups_directions_directionid");

            entity.HasOne(d => d.Project).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_groups_projects_projectid");

            entity.HasMany(d => d.Students).WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentGroup",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("Studentsid")
                        .HasConstraintName("fk_student_groups_students_studentsid"),
                    l => l.HasOne<Group>().WithMany()
                        .HasForeignKey("Groupsid")
                        .HasConstraintName("fk_student_groups_groups_groupsid"),
                    j =>
                    {
                        j.HasKey("Groupsid", "Studentsid").HasName("pk_student_groups");
                        j.ToTable("student_groups");
                        j.HasIndex(new[] { "Studentsid" }, "ix_student_groups_studentsid");
                        j.IndexerProperty<int>("Groupsid").HasColumnName("groupsid");
                        j.IndexerProperty<int>("Studentsid").HasColumnName("studentsid");
                    });

            entity.HasMany(d => d.Tests).WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "TestGroup",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("Testsid")
                        .HasConstraintName("fk_test_groups_tests_testsid"),
                    l => l.HasOne<Group>().WithMany()
                        .HasForeignKey("Groupsid")
                        .HasConstraintName("fk_test_groups_groups_groupsid"),
                    j =>
                    {
                        j.HasKey("Groupsid", "Testsid").HasName("pk_test_groups");
                        j.ToTable("test_groups");
                        j.HasIndex(new[] { "Testsid" }, "ix_test_groups_testsid");
                        j.IndexerProperty<int>("Groupsid").HasColumnName("groupsid");
                        j.IndexerProperty<int>("Testsid").HasColumnName("testsid");
                    });
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_projects");

            entity.ToTable("projects");

            entity.HasIndex(e => e.Name, "ix_projects_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasMany(d => d.Tests).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "TestProject",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("Testsid")
                        .HasConstraintName("fk_test_projects_tests_testsid"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("Projectsid")
                        .HasConstraintName("fk_test_projects_projects_projectsid"),
                    j =>
                    {
                        j.HasKey("Projectsid", "Testsid").HasName("pk_test_projects");
                        j.ToTable("test_projects");
                        j.HasIndex(new[] { "Testsid" }, "ix_test_projects_testsid");
                        j.IndexerProperty<int>("Projectsid").HasColumnName("projectsid");
                        j.IndexerProperty<int>("Testsid").HasColumnName("testsid");
                    });
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_questions");

            entity.ToTable("questions");

            entity.HasIndex(e => new { e.Testid, e.Number }, "ix_questions_testid_number").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answertype).HasColumnName("answertype");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnName("description");
            entity.Property(e => e.Isscoring)
                .HasDefaultValue(true)
                .HasColumnName("isscoring");
            entity.Property(e => e.Maxscore).HasColumnName("maxscore");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Testid).HasColumnName("testid");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Testid)
                .HasConstraintName("fk_questions_tests_testid");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_refreshtokens");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.Tokenhash, "ix_refreshtokens_tokenhash");

            entity.HasIndex(e => e.Userid, "ix_refreshtokens_userid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Expiresat).HasColumnName("expiresat");
            entity.Property(e => e.Revokedat).HasColumnName("revokedat");
            entity.Property(e => e.Tokenhash).HasColumnName("tokenhash");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_refreshtokens_users_userid");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_students");

            entity.ToTable("students");

            entity.HasIndex(e => e.Userid, "ix_students_userid").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatarpath).HasColumnName("avatarpath");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Vkprofilelink).HasColumnName("vkprofilelink");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.Userid)
                .HasConstraintName("fk_students_users_userid");

            entity.HasMany(d => d.Tests).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "TestStudent",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("Testsid")
                        .HasConstraintName("fk_test_students_tests_testsid"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("Studentsid")
                        .HasConstraintName("fk_test_students_students_studentsid"),
                    j =>
                    {
                        j.HasKey("Studentsid", "Testsid").HasName("pk_test_students");
                        j.ToTable("test_students");
                        j.HasIndex(new[] { "Testsid" }, "ix_test_students_testsid");
                        j.IndexerProperty<int>("Studentsid").HasColumnName("studentsid");
                        j.IndexerProperty<int>("Testsid").HasColumnName("testsid");
                    });
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tests");

            entity.ToTable("tests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property(e => e.Deadline).HasColumnName("deadline");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Durationminutes).HasColumnName("durationminutes");
            entity.Property(e => e.Ispublic)
                .HasDefaultValue(false)
                .HasColumnName("ispublic");
            entity.Property(e => e.Isrepeatable)
                .HasDefaultValue(false)
                .HasColumnName("isrepeatable");
            entity.Property(e => e.Maxattempts).HasColumnName("maxattempts");
            entity.Property(e => e.Passingscore).HasColumnName("passingscore");
            entity.Property(e => e.Publishedat).HasColumnName("publishedat");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Testresult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_testresults");

            entity.ToTable("testresults");

            entity.HasIndex(e => e.Attemptid, "ix_testresults_attemptid");

            entity.HasIndex(e => e.Studentid, "ix_testresults_studentid");

            entity.HasIndex(e => new { e.Testid, e.Studentid, e.Attemptid }, "ix_testresults_testid_studentid_attemptid").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attemptid).HasColumnName("attemptid");
            entity.Property(e => e.Passed).HasColumnName("passed");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
            entity.Property(e => e.Testid).HasColumnName("testid");

            entity.HasOne(d => d.Attempt).WithMany(p => p.Testresults)
                .HasForeignKey(d => d.Attemptid)
                .HasConstraintName("fk_testresults_attempts_attemptid");

            entity.HasOne(d => d.Student).WithMany(p => p.Testresults)
                .HasForeignKey(d => d.Studentid)
                .HasConstraintName("fk_testresults_students_studentid");

            entity.HasOne(d => d.Test).WithMany(p => p.Testresults)
                .HasForeignKey(d => d.Testid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_testresults_tests_testid");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_users");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "ix_users_email").IsUnique();

            entity.HasIndex(e => e.Login, "ix_users_login").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Middlename).HasColumnName("middlename");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Userattemptanswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_userattemptanswers");

            entity.ToTable("userattemptanswers");

            entity.HasIndex(e => new { e.Attemptid, e.Questionid }, "ix_userattemptanswers_attemptid_questionid").IsUnique();

            entity.HasIndex(e => e.Questionid, "ix_userattemptanswers_questionid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attemptid).HasColumnName("attemptid");
            entity.Property(e => e.Iscorrect).HasColumnName("iscorrect");
            entity.Property(e => e.Questionid).HasColumnName("questionid");
            entity.Property(e => e.Scoreawarded).HasColumnName("scoreawarded");

            entity.HasOne(d => d.Attempt).WithMany(p => p.Userattemptanswers)
                .HasForeignKey(d => d.Attemptid)
                .HasConstraintName("fk_userattemptanswers_attempts_attemptid");

            entity.HasOne(d => d.Question).WithMany(p => p.Userattemptanswers)
                .HasForeignKey(d => d.Questionid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_userattemptanswers_questions_questionid");
        });

        modelBuilder.Entity<Userselectedoption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_userselectedoptions");

            entity.ToTable("userselectedoptions");

            entity.HasIndex(e => e.Answerid, "ix_userselectedoptions_answerid");

            entity.HasIndex(e => e.Userattemptanswerid, "ix_userselectedoptions_userattemptanswerid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answerid).HasColumnName("answerid");
            entity.Property(e => e.Userattemptanswerid).HasColumnName("userattemptanswerid");

            entity.HasOne(d => d.Answer).WithMany(p => p.Userselectedoptions)
                .HasForeignKey(d => d.Answerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_userselectedoptions_answers_answerid");

            entity.HasOne(d => d.Userattemptanswer).WithMany(p => p.Userselectedoptions)
                .HasForeignKey(d => d.Userattemptanswerid)
                .HasConstraintName("fk_userselectedoptions_userattemptanswers_userattemptanswerid");
        });

        modelBuilder.Entity<Usertextanswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_usertextanswers");

            entity.ToTable("usertextanswers");

            entity.HasIndex(e => e.Userattemptanswerid, "ix_usertextanswers_userattemptanswerid").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Textanswer).HasColumnName("textanswer");
            entity.Property(e => e.Userattemptanswerid).HasColumnName("userattemptanswerid");

            entity.HasOne(d => d.Userattemptanswer).WithOne(p => p.Usertextanswer)
                .HasForeignKey<Usertextanswer>(d => d.Userattemptanswerid)
                .HasConstraintName("fk_usertextanswers_userattemptanswers_userattemptanswerid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
