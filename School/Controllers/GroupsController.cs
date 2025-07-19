using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.Models;

namespace OnlineSchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups()
    {
        return await _context.Groups
            .Include(g => g.Teacher)
            .ToListAsync();
    }

    // GET: api/groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Teacher)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
            return NotFound();

        return group;
    }

    // POST: api/groups
    [HttpPost]
    public async Task<ActionResult<Group>> CreateGroup(Group group)
    {
        var teacher = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == group.TeacherId && u.Role == "Teacher");

        if (teacher == null)
            return BadRequest("Указанный преподаватель не найден");

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
    }

    // PUT: api/groups/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, Group updatedGroup)
    {
        if (id != updatedGroup.Id)
            return BadRequest();

        _context.Entry(updatedGroup).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok("Группа обновлена");
    }

    // DELETE: api/groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return NotFound();

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return Ok("Группа удалена");
    }
}
