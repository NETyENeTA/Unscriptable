using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using Unscriptable.Application.DTOs.Dapper.Filters;
using Unscriptable.Application.DTOs.Dapper.Reports;
using Unscriptable.Application.Interfaces;
using Unscriptable.Domain.Enums;
using Unscriptable.Infrastructure.Reports;

namespace Unscriptable.Infrastructure.Services;

public class ReportsService : IReportsService
{
    private readonly IDbConnection _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CultureInfo _culture = new CultureInfo("ru-RU");

    public ReportsService(IDbConnection db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    // 1. Активность по дням недели
    public async Task<IEnumerable<DayOfWeekActivityItem>> GetDayOfWeekActivityAsync(DayOfWeekActivityFilter f)
    {
        CheckStaffAccess(); // Только учителя и админы
        return await _db.QueryAsync<DayOfWeekActivityItem>(ReportQueries.DayOfWeekActivity, f);
    }

    // 2. Лидеры и отстающие групп
    public async Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter f)
    {
        CheckStaffAccess();
        return await _db.QueryAsync<GroupLeadersAndLaggardsItem>(ReportQueries.LeadersAndLaggards, f);
    }

    // 3. Динамика среднего балла (Тренды)
    public async Task<IEnumerable<GroupTrendItem>> GetGroupTrendAsync(GroupTrendFilter f)
    {
        CheckStaffAccess();
        var data = await _db.QueryAsync<GroupTrendItem>(ReportQueries.GroupTrend, f);
        foreach (var i in data)
            i.MonthLabel = i.Month.ToString("MMM yyyy", _culture);
        return data;
    }

    // 4. Накопленные баллы студента по месяцам
    public async Task<IEnumerable<StudentMonthlyProgressItem>> GetStudentMonthlyProgressAsync(StudentMonthlyProgressFilter f)
    {
        CheckStudentOwnership(f.StudentId); // Студент видит только своё
        var data = await _db.QueryAsync<StudentMonthlyProgressItem>(ReportQueries.StudentMonthlyProgress, f);
        foreach (var i in data)
            i.MonthLabel = i.Month.ToString("MMM yyyy", _culture);
        return data;
    }

    // 5. Процент прохождения тестов (Общий список)
    public async Task<IEnumerable<StudentPassRateItem>> GetStudentPassRateAsync(StudentPassRateFilter f)
    {
        CheckStaffAccess();
        return await _db.QueryAsync<StudentPassRateItem>(ReportQueries.StudentPassRate, f);
    }

    // 6. Сводка по конкретному студенту (Pass Rate)
    public async Task<StudentPassRateSummaryItem?> GetStudentPassRateSummaryAsync(StudentPassRateSummaryFilter f)
    {
        CheckStudentOwnership(f.StudentId);
        return await _db.QueryFirstOrDefaultAsync<StudentPassRateSummaryItem>(ReportQueries.StudentPassRateSummary, f);
    }

    // 7. Общий рейтинг студентов
    public async Task<IEnumerable<StudentRatingItem>> GetStudentRatingAsync(StudentRatingFilter f)
    {
        CheckStaffAccess();
        return await _db.QueryAsync<StudentRatingItem>(ReportQueries.StudentRating, f);
    }

    // 8. Подробные результаты тестов ученика
    public async Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter f)
    {
        CheckStudentOwnership(f.StudentId);
        return await _db.QueryAsync<StudentTestResultsItem>(ReportQueries.StudentTestResults, f);
    }

    // --- Приватные методы проверки прав ---

    private void CheckStaffAccess()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity!.IsAuthenticated)
            throw new UnauthorizedAccessException("Требуется авторизация.");

        if (!user.IsInRole(UserRole.Manager.ToString()) && !user.IsInRole("Teacher"))
            throw new UnauthorizedAccessException("Доступ запрещен. Требуется роль Преподавателя или Администратора.");
    }

    private void CheckStudentOwnership(int targetStudentId)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        // 1. Проверяем, авторизован ли пользователь вообще
        if (user == null || !user.Identity!.IsAuthenticated)
            throw new UnauthorizedAccessException("Требуется авторизация.");

        // 2. Если это Администратор или Учитель — разрешаем доступ к любому ID
        if (user.IsInRole(UserRole.Manager.ToString()) || user.IsInRole("Teacher"))
            return;

        // 3. Если это Студент — достаем его StudentId из клеймов
        var claimStudentId = user.FindFirst("StudentId")?.Value;

        // Сравниваем ID из запроса с ID из токена/куки
        if (string.IsNullOrEmpty(claimStudentId) || targetStudentId.ToString() != claimStudentId)
        {
            throw new UnauthorizedAccessException("Вы можете просматривать только свои данные.");
        }
    }
}