namespace School.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Student"; // Student / Teacher / Admin

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигация
    public Student? StudentProfile { get; set; }
    public ICollection<Group>? TeachingGroups { get; set; }
}