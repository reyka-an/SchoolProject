using Microsoft.EntityFrameworkCore;
using School.Models;

namespace School.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Первичные данные для статусов посещения
        modelBuilder.Entity<AttendanceStatus>().HasData(
            new AttendanceStatus { Id = 1, Code = "present", Label = "Был" },
            new AttendanceStatus { Id = 2, Code = "sick", Label = "Болел" },
            new AttendanceStatus { Id = 3, Code = "missed", Label = "Не был" },
            new AttendanceStatus { Id = 4, Code = "makeup", Label = "Отработка" },
            new AttendanceStatus { Id = 5, Code = "trial", Label = "Пробное" }
        );
    }
}