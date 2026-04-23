using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unscriptable.Application.DTOs;
using Unscriptable.Domain.Enums;
using Unscriptable.Infrastructure.Data;

namespace Unscriptable.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PeopleController(AppDbContext context) : ControllerBase
{
    [HttpGet("all-students")]
    public async Task<IActionResult> GetStudents()
    {
        if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
        {
            return Forbid();
        }

        var students = await context.Students
            .Include(s => s.User)
            .Select(s => new UserProfileDto
            {
                Id = s.Id,
                Login = s.User.Login,
                Email = s.User.Email,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName
            })
            .ToListAsync();

        return Ok(students);
    }

    [HttpPut("update-profile/{id:int}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserProfileDto dto)
    {
        // Только админ может менять чужие профили
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var student = await context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        if (student == null) return NotFound();

        student.User.Email = dto.Email;
        student.User.FirstName = dto.FirstName;
        student.User.LastName = dto.LastName;

        await context.SaveChangesAsync();
        return Ok("Профиль обновлен");
    }
}