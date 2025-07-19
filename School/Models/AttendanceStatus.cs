namespace OnlineSchool.Models;

public class AttendanceStatus
{
    public int Id { get; set; }

    // Для внутренней логики
    public string Code { get; set; } = string.Empty;

    // Отображается в UI
    public string Label { get; set; } = string.Empty;

    // Дополнительное описание
    public string? Description { get; set; }

    // Навигация
    public ICollection<Attendance>? Attendances { get; set; }
}