namespace School.Dto;

public record StudentDto(
    int Id,
    string FullName,
    string Email,
    string PhoneNumber,
    bool HadTrial,
    bool IsHidden,
    DateTime CreatedAt,
    IEnumerable<string> Groups
);
