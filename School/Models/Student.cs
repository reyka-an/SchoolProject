namespace School.Models;

public class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool HadTrial { get; set; } = false;
    
    public bool IsHidden { get; set; } = false;

    // Навигация
    public User User { get; set; } = null!;
    public ICollection<StudentGroup>? StudentGroups { get; set; }
    public ICollection<Attendance>? Attendances { get; set; }
}