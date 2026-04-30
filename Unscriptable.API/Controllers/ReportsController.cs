using Microsoft.AspNetCore.Mvc;
using Unscriptable.Application.DTOs.Dapper.Filters;
using Unscriptable.Application.Interfaces;

namespace Unscriptable.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _service;

    public ReportsController(IReportsService service)
    {
        _service = service;
    }

    // 1. Активность по дням недели
    [HttpGet("activity")]
    public async Task<IActionResult> GetActivity([FromQuery] DayOfWeekActivityFilter filter)
    {
        var result = await _service.GetDayOfWeekActivityAsync(filter);
        return Ok(result);
    }

    // 2. Лидеры и отстающие в группах
    [HttpGet("leaders-and-laggards")]
    public async Task<IActionResult> GetLeaders([FromQuery] GroupLeadersAndLaggardsFilter filter)
    {
        var result = await _service.GetLeadersAndLaggardsAsync(filter);
        return Ok(result);
    }

    // 3. Динамика среднего балла (Тренды)
    [HttpGet("group-trend")]
    public async Task<IActionResult> GetGroupTrend([FromQuery] GroupTrendFilter filter)
    {
        var result = await _service.GetGroupTrendAsync(filter);
        return Ok(result);
    }

    // 4. Накопленные баллы студента
    [HttpGet("student-progress")]
    public async Task<IActionResult> GetStudentProgress([FromQuery] StudentMonthlyProgressFilter filter)
    {
        if (filter.StudentId <= 0) return BadRequest("Некорректный StudentId");

        var result = await _service.GetStudentMonthlyProgressAsync(filter);
        return Ok(result);
    }

    // 5. Общий Pass Rate по всем студентам
    [HttpGet("pass-rate-all")]
    public async Task<IActionResult> GetPassRateAll([FromQuery] StudentPassRateFilter filter)
    {
        var result = await _service.GetStudentPassRateAsync(filter);
        return Ok(result);
    }

    // 6. Сводка (Pass Rate) по конкретному студенту
    [HttpGet("student-summary")]
    public async Task<IActionResult> GetStudentSummary([FromQuery] StudentPassRateSummaryFilter filter)
    {
        if (filter.StudentId <= 0) return BadRequest("Некорректный StudentId");

        var result = await _service.GetStudentPassRateSummaryAsync(filter);
        return result != null ? Ok(result) : NotFound("Студент не найден или нет данных");
    }

    // 7. Общий рейтинг студентов
    [HttpGet("rating")]
    public async Task<IActionResult> GetRating([FromQuery] StudentRatingFilter filter)
    {
        var result = await _service.GetStudentRatingAsync(filter);
        return Ok(result);
    }

    // 8. Все результаты тестов конкретного ученика
    [HttpGet("student-test-results")]
    public async Task<IActionResult> GetStudentTestResults([FromQuery] StudentTestResultsFilter filter)
    {
        if (filter.StudentId <= 0) return BadRequest("Некорректный StudentId");

        var result = await _service.GetStudentTestResultsAsync(filter);
        return Ok(result);
    }
}
