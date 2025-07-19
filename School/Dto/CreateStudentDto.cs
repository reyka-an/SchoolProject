namespace School.Dto;

public record CreateStudentDto(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password,      // временный
    bool HadTrial = false
);