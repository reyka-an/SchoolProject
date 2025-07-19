namespace School.Models;

public class Attendance
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public int StudentId { get; set; }

    public int StatusId { get; set; }

    public AttendanceStatus Status { get; set; } = null!;
    
    public string? Comment { get; set; }

    // Навигация
    public Lesson Lesson { get; set; } = null!;
    public Student Student { get; set; } = null!;
}