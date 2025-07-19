using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.Models;

namespace OnlineSchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentGroupsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/studentgroups/group/1
    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IEnumerable<StudentGroup>>> GetStudentsInGroup(int groupId)
    {
        var students = await _context.StudentGroups
            .Include(sg => sg.Student)
                .ThenInclude(s => s.User)
            .Where(sg => sg.GroupId == groupId)
            .ToListAsync();

        return students;
    }

    // POST: api/studentgroups
    [HttpPost]
    public async Task<ActionResult<StudentGroup>> AddStudentToGroup(StudentGroup studentGroup)
    {
        // Проверка: существует ли студент и группа
        var student = await _context.Students.FindAsync(studentGroup.StudentId);
        var group = await _context.Groups.FindAsync(studentGroup.GroupId);

        if (student == null || group == null)
            return BadRequest("Студент или группа не найдены");

        // Проверка: уже состоит в группе?
        bool exists = await _context.StudentGroups
            .AnyAsync(sg => sg.StudentId == studentGroup.StudentId && sg.GroupId == studentGroup.GroupId);

        if (exists)
            return Conflict("Студент уже состоит в этой группе");

        _context.StudentGroups.Add(studentGroup);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudentsInGroup), new { groupId = studentGroup.GroupId }, studentGroup);
    }

    // DELETE: api/studentgroups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveStudentFromGroup(int id)
    {
        var link = await _context.StudentGroups.FindAsync(id);
        if (link == null)
            return NotFound();

        _context.StudentGroups.Remove(link);
        await _context.SaveChangesAsync();

        return Ok("Ученик удалён из группы");
    }
}
