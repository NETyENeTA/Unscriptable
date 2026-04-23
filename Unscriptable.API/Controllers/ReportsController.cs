using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unscriptable.Application.DTOs.Filters;
using Unscriptable.Application.Interfaces;
using Unscriptable.Domain.Entitties;

namespace Unscriptable.API.Controllers;

[Authorize] // Требовать авторизацию для всех методов
[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Извлекает ID текущего пользователя из Claims токена (JWT или Cookie)
    /// </summary>
    private int CurrentUserId =>
        int.TryParse(User.FindFirst("id")?.Value, out var id) ? id : 0;

    /// <summary>
    /// Проверяет, является ли текущий пользователь Менеджером
    /// </summary>
    private bool IsManager => User.IsInRole("Manager");

    [HttpGet("group-leaders")]
    public async Task<IActionResult> GetGroupLeaders([FromQuery] GroupLeadersAndLaggardsFilter filter)
    {
        // Отчет доступен всем авторизованным пользователям
        var result = await reportService.GetGroupLeadersAndLaggardsAsync(filter);
        return Ok(result);
    }

    [HttpGet("student-test-results")]
    public async Task<IActionResult> GetStudentTestResults([FromQuery] StudentTestResultsFilter filter)
    {
        // Защита: Студент может смотреть только свои результаты
        if (!IsManager && filter.StudentId != CurrentUserId)
        {
            return Forbid(); // 403 Forbidden
        }

        var result = await reportService.GetStudentTestResultsAsync(filter);
        return Ok(result);
    }

    [HttpGet("group-trend")]
    public async Task<IActionResult> GetGroupTrend([FromQuery] GroupTrendFilter filter)
    {
        // Только менеджеры могут видеть тренды групп (согласно конспекту №6)
        if (!IsManager) return Forbid();

        var result = await reportService.GetGroupTrendAsync(filter);
        return Ok(result);
    }

    [HttpGet("student-rating")]
    public async Task<IActionResult> GetStudentRating([FromQuery] StudentRatingFilter filter)
    {
        var result = await reportService.GetStudentRatingAsync(filter);
        return Ok(result);
    }

    [HttpGet("student-monthly-progress")]
    public async Task<IActionResult> GetStudentMonthlyProgress([FromQuery] StudentMonthlyProgressFilter filter)
    {
        // Если ID не передан, берем ID текущего пользователя
        if (filter.StudentId == 0) filter.StudentId = CurrentUserId;

        // Студент не может смотреть чужой прогресс
        if (!IsManager && filter.StudentId != CurrentUserId) return Forbid();

        var result = await reportService.GetStudentMonthlyProgressAsync(filter);
        return Ok(result);
    }

    [HttpGet("student-pass-rate")]
    public async Task<IActionResult> GetStudentPassRate([FromQuery] StudentPassRateFilter filter)
    {
        if (!IsManager && filter.StudentId != CurrentUserId) return Forbid();

        var result = await reportService.GetStudentPassRateAsync(filter);
        return Ok(result);
    }

    [HttpGet("student-summary")]
    public async Task<IActionResult> GetStudentSummary([FromQuery] StudentPassRateSummaryFilter filter)
    {
        // Сводная карточка по студенту
        if (!IsManager && filter.StudentId != CurrentUserId) return Forbid();

        var result = await reportService.GetStudentPassRateSummaryAsync(filter);
        return Ok(result);
    }

    [HttpGet("activity-days")]
    public async Task<IActionResult> GetDayOfWeekActivity([FromQuery] DayOfWeekActivityFilter filter)
    {
        // Общая статистика активности (обычно для менеджеров)
        if (!IsManager) return Forbid();

        var result = await reportService.GetDayOfWeekActivityAsync(filter);
        return Ok(result);
    }
}