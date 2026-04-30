using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Application.DTOs.Dapper.Filters;
using Unscriptable.Application.DTOs.Dapper.Reports;
using Unscriptable.Application.Interfaces;
using Unscriptable.Infrastructure.Reports;

namespace Unscriptable.Infrastructure.Services;

public class ReportsService : IReportsService
{
    private readonly IDbConnection _db;
    private readonly CultureInfo _culture = new CultureInfo("ru-RU");

    // IDbConnection внедряется через DI из Program.cs
    public ReportsService(IDbConnection db)
    {
        _db = db;
    }

    // 1. Активность по дням недели
    public async Task<IEnumerable<DayOfWeekActivityItem>> GetDayOfWeekActivityAsync(DayOfWeekActivityFilter f)
    {
        var data = await _db.QueryAsync<DayOfWeekActivityItem>(ReportQueries.DayOfWeekActivity, f);
        //foreach (var i in data)
        //    i.DayName = _culture.DateTimeFormat.GetAbbreviatedDayName((DayOfWeek)i.DayOfWeek);
        return data;
    }

    // 2. Лидеры и отстающие групп
    public async Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter f)
    {
        return await _db.QueryAsync<GroupLeadersAndLaggardsItem>(ReportQueries.LeadersAndLaggards, f);
    }

    // 3. Динамика среднего балла (Тренды)
    public async Task<IEnumerable<GroupTrendItem>> GetGroupTrendAsync(GroupTrendFilter f)
    {
        var data = await _db.QueryAsync<GroupTrendItem>(ReportQueries.GroupTrend, f);
        foreach (var i in data)
            i.MonthLabel = i.Month.ToString("MMM yyyy", _culture);
        return data;
    }

    // 4. Накопленные баллы студента по месяцам
    public async Task<IEnumerable<StudentMonthlyProgressItem>> GetStudentMonthlyProgressAsync(StudentMonthlyProgressFilter f)
    {
        var data = await _db.QueryAsync<StudentMonthlyProgressItem>(ReportQueries.StudentMonthlyProgress, f);
        foreach (var i in data)
            i.MonthLabel = i.Month.ToString("MMM yyyy", _culture);
        return data;
    }

    // 5. Процент прохождения тестов (Общий список)
    public async Task<IEnumerable<StudentPassRateItem>> GetStudentPassRateAsync(StudentPassRateFilter f)
    {
        return await _db.QueryAsync<StudentPassRateItem>(ReportQueries.StudentPassRate, f);
    }

    // 6. Сводка по конкретному студенту (Pass Rate)
    public async Task<StudentPassRateSummaryItem?> GetStudentPassRateSummaryAsync(StudentPassRateSummaryFilter f)
    {
        return await _db.QueryFirstOrDefaultAsync<StudentPassRateSummaryItem>(ReportQueries.StudentPassRateSummary, f);
    }

    // 7. Общий рейтинг студентов
    public async Task<IEnumerable<StudentRatingItem>> GetStudentRatingAsync(StudentRatingFilter f)
    {
        return await _db.QueryAsync<StudentRatingItem>(ReportQueries.StudentRating, f);
    }

    // 8. Подробные результаты тестов ученика
    public async Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter f)
    {
        return await _db.QueryAsync<StudentTestResultsItem>(ReportQueries.StudentTestResults, f);
    }
}
