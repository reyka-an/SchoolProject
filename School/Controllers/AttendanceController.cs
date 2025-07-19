using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;

namespace School.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttendanceController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/attendance/lesson/5
    [HttpGet("lesson/{lessonId}")]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetByLesson(int lessonId)
    {
        return await _context.Attendances
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Include(a => a.Status)
            .Where(a => a.LessonId == lessonId)
            .ToListAsync();
    }

    // POST: api/attendance
    [HttpPost]
    public async Task<ActionResult<Attendance>> CreateAttendance(Attendance attendance)
    {
        var lesson = await _context.Lessons.FindAsync(attendance.LessonId);
        var student = await _context.Students.FindAsync(attendance.StudentId);
        var status = await _context.AttendanceStatuses.FindAsync(attendance.StatusId);

        if (lesson == null || student == null || status == null)
            return BadRequest("Неверный lessonId, studentId или statusId");

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByLesson), new { lessonId = attendance.LessonId }, attendance);
    }

    // PUT: api/attendance/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, Attendance updated)
    {
        if (id != updated.Id)
            return BadRequest();

        _context.Entry(updated).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok("Отметка обновлена");
    }

    // DELETE: api/attendance/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
            return NotFound();

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();

        return Ok("Отметка удалена");
    }
}
