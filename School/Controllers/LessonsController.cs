using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.Models;

namespace OnlineSchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LessonsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/lessons
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetAllLessons()
    {
        return await _context.Lessons
            .Include(l => l.Group)
            .Include(l => l.Teacher)
            .ToListAsync();
    }

    // GET: api/lessons/group/1
    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonsByGroup(int groupId)
    {
        return await _context.Lessons
            .Include(l => l.Teacher)
            .Where(l => l.GroupId == groupId)
            .ToListAsync();
    }

    // GET: api/lessons/teacher/2
    [HttpGet("teacher/{teacherId}")]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonsByTeacher(int teacherId)
    {
        return await _context.Lessons
            .Include(l => l.Group)
            .Where(l => l.TeacherId == teacherId)
            .ToListAsync();
    }

    // GET: api/lessons/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Lesson>> GetLesson(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Group)
            .Include(l => l.Teacher)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lesson == null)
            return NotFound();

        return lesson;
    }

    // POST: api/lessons
    [HttpPost]
    public async Task<ActionResult<Lesson>> CreateLesson(Lesson lesson)
    {
        // Проверка: преподаватель должен существовать и иметь роль "Teacher"
        var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == lesson.TeacherId && u.Role == "Teacher");
        var group = await _context.Groups.FindAsync(lesson.GroupId);

        if (teacher == null || group == null)
            return BadRequest("Группа или преподаватель не найдены");

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
    }

    // PUT: api/lessons/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(int id, Lesson updatedLesson)
    {
        if (id != updatedLesson.Id)
            return BadRequest();

        _context.Entry(updatedLesson).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok("Занятие обновлено");
    }

    // DELETE: api/lessons/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
            return NotFound();

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();

        return Ok("Занятие удалено");
    }
}
