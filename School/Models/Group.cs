namespace School.Models;

public class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int TeacherId { get; set; }

    // Навигация
    public User Teacher { get; set; } = null!;
    public ICollection<StudentGroup>? StudentGroups { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
}