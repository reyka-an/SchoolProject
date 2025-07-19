namespace OnlineSchool.Models;

public class StudentGroup
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Навигация
    public Student Student { get; set; } = null!;
    public Group Group { get; set; } = null!;
}