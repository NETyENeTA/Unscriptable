using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unscriptable.Application.DTOs;
using Unscriptable.Domain.Entitties;
using Unscriptable.Infrastructure.Data;

namespace Unscriptable.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tests = await context.Tests.ToListAsync();
        return Ok(tests);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var test = await context.Tests.FindAsync(id);
        if (test == null) return NotFound("Тест не найден");
        return Ok(test);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TestDto dto)
    {
        // Ручная проверка роли
        if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
        {
            return Forbid(); // 403 доступ запрещен
        }

        var test = new Test
        {
            Title = dto.Title,
            Description = dto.Description,
            MaxScore = dto.MaxScore
        };

        context.Tests.Add(test);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = test.Id }, test);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var test = await context.Tests.FindAsync(id);
        if (test == null) return NotFound();

        context.Tests.Remove(test);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
