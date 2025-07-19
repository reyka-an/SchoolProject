namespace School.Models;

public class Lesson
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int TeacherId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Notes { get; set; }

    // Навигация
    public Group Group { get; set; } = null!;
    public User Teacher { get; set; } = null!;
    public ICollection<Attendance>? Attendances { get; set; }
}