using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Unscriptable.Application.DTOs;
using Unscriptable.Domain.Entitties;
using Unscriptable.Infrastructure.Data;

namespace Unscriptable.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttemptsController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAttempt([FromBody] AttemptDto dto)
    {
        // Проверяем, что роль — Студент
        if (!User.IsInRole("Student"))
        {
            return BadRequest("Только студенты могут проходить тесты");
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);

        // Находим профиль студента по UserId
        var student = await context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        if (student == null) return BadRequest("Профиль студента не найден в БД");

        var attempt = new Attempt
        {
            TestId = dto.TestId,
            StudentId = student.Id,
            Score = dto.Score,
            StartedAt = DateTimeOffset.UtcNow,
            SubmittedAt = DateTimeOffset.UtcNow
        };

        context.Attempts.Add(attempt);
        await context.SaveChangesAsync();
        return Ok(attempt);
    }

    [HttpGet("my-history")]
    public async Task<IActionResult> GetMyHistory()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var attempts = await context.Attempts
            .Include(a => a.Test)
            .Where(a => a.Student.UserId == userId)
            .OrderByDescending(a => a.SubmittedAt)
            .ToListAsync();

        return Ok(attempts);
    }
}