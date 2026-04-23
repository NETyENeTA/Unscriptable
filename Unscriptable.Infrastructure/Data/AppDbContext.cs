using Microsoft.EntityFrameworkCore;
using Unscriptable.Domain.Entitties;


namespace Unscriptable.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Attempt> Attempts => Set<Attempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Groups)
            .WithMany(g => g.Students)
            .UsingEntity<Dictionary<string, object>>(
                "student_groups", // Имя таблицы в БД
                j => j.HasOne<Group>().WithMany().HasForeignKey("groupsid"),
                j => j.HasOne<Student>().WithMany().HasForeignKey("studentsid")
            );

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Student>().ToTable("students");
        modelBuilder.Entity<Group>().ToTable("groups");
        modelBuilder.Entity<Test>().ToTable("tests");
        modelBuilder.Entity<Attempt>().ToTable("attempts");

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasColumnName("firstname");
            entity.Property(e => e.LastName).HasColumnName("lastname");
            entity.Property(e => e.MiddleName).HasColumnName("middlename");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.Property(e => e.MaxScore).HasColumnName("maxscore");
        });

        modelBuilder.Entity<Attempt>(entity =>
        {
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StartedAt).HasColumnName("startedat");
            entity.Property(e => e.SubmittedAt).HasColumnName("submittedat");
        });

        // Настройка связи User -> Student (1 к 0..1)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId);
    }
}