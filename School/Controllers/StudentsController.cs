using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;
using School.Dto;

namespace School.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/students?showHidden=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery] bool showHidden = false)
    {
        var query = _context.Students
            .Include(s => s.User)
            .Include(s => s.StudentGroups!)
                .ThenInclude(sg => sg.Group)
            .AsQueryable();

        if (!showHidden)
            query = query.Where(s => !s.IsHidden);

        var result = await query
            .Select(s => new StudentDto(
                s.Id,
                s.User.FullName,
                s.User.Email,
                s.User.PhoneNumber,
                s.HadTrial,
                s.IsHidden,
                s.User.CreatedAt,
                s.StudentGroups!.Select(g => g.Group.Name)
            ))
            .ToListAsync();

        return result;
    }

    // GET: api/students/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.StudentGroups!)
                .ThenInclude(sg => sg.Group)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student is null)
            return NotFound();

        return new StudentDto(
            student.Id,
            student.User.FullName,
            student.User.Email,
            student.User.PhoneNumber,
            student.HadTrial,
            student.IsHidden,
            student.User.CreatedAt,
            student.StudentGroups!.Select(g => g.Group.Name));
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto dto)
    {
        // уникальная почта
        bool emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailTaken)
            return Conflict("Пользователь с такой почтой уже существует");

        var user = new User
        {
            FullName     = dto.FullName,
            Email        = dto.Email,
            PhoneNumber  = dto.PhoneNumber,
            Role         = "Student",
            // TODO: захэшировать пароль; пока просто временная заглушка
            PasswordHash = dto.Password
        };

        var student = new Student
        {
            User        = user,
            HadTrial    = dto.HadTrial
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, new StudentDto(
            student.Id,
            user.FullName,
            user.Email,
            user.PhoneNumber,
            student.HadTrial,
            student.IsHidden,
            user.CreatedAt,
            Array.Empty<string>()));
    }

    // PUT: api/students/5   (изменить профиль или скрыть/открыть)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student is null) return NotFound();

        student.User.FullName   = dto.FullName;
        student.User.Email      = dto.Email;
        student.User.PhoneNumber= dto.PhoneNumber;
        student.HadTrial        = dto.HadTrial;
        student.IsHidden        = dto.IsHidden;

        await _context.SaveChangesAsync();
        return Ok("Ученик обновлён");
    }

    // DELETE: api/students/5  (полное удаление, используйте осторожно)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null) return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return Ok("Ученик удалён");
    }
}
