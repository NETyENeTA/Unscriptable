using Microsoft.EntityFrameworkCore;
using Unscriptable.Domain.Entitties;


namespace Unscriptable.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Attempt> Attempts => Set<Attempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Login).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(s => s.Phone).HasMaxLength(30);

            entity.HasOne(s => s.User)
                  .WithOne(u => u.Student)
                  .HasForeignKey<Student>(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Настройка Attempt (Связи и значения по умолчанию)
        modelBuilder.Entity<Attempt>(entity =>
        {
            entity.Property(a => a.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Связь с Тестом (Restrict - нельзя удалить тест, если есть попытки)
            entity.HasOne(a => a.Test)
                  .WithMany(t => t.Attempts)
                  .HasForeignKey(a => a.TestId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Связь со Студентом (Cascade - удалили студента, удалились попытки)
            entity.HasOne(a => a.Student)
                  .WithMany(s => s.Attempts)
                  .HasForeignKey(a => a.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}