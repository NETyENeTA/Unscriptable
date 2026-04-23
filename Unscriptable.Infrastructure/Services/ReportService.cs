using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Application.DTOs.Dapper;
using Unscriptable.Application.DTOs.Filters;
using Unscriptable.Application.Interfaces;
using Unscriptable.Infrastructure.Reports;

namespace Unscriptable.Infrastructure.Services;

public class ReportService(IDbConnection connection) : IReportService
{
    public async Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetGroupLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter filter) =>
        await connection.QueryAsync<GroupLeadersAndLaggardsItem>(ReportQueries.GroupLeadersAndLaggards, filter);

    public async Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter filter) =>
        await connection.QueryAsync<StudentTestResultsItem>(ReportQueries.StudentTestResults, filter);

    public async Task<IEnumerable<GroupTrendItem>> GetGroupTrendAsync(GroupTrendFilter filter) =>
        await connection.QueryAsync<GroupTrendItem>(ReportQueries.GroupTrend, filter);

    public async Task<IEnumerable<StudentRatingItem>> GetStudentRatingAsync(StudentRatingFilter filter) =>
        await connection.QueryAsync<StudentRatingItem>(ReportQueries.StudentRating, filter);

    public async Task<IEnumerable<StudentMonthlyProgressItem>> GetStudentMonthlyProgressAsync(StudentMonthlyProgressFilter filter) =>
        await connection.QueryAsync<StudentMonthlyProgressItem>(ReportQueries.StudentMonthlyProgress, filter);

    public async Task<IEnumerable<StudentPassRateItem>> GetStudentPassRateAsync(StudentPassRateFilter filter) =>
        await connection.QueryAsync<StudentPassRateItem>(ReportQueries.StudentPassRate, filter);

    public async Task<StudentPassRateSummaryItem?> GetStudentPassRateSummaryAsync(StudentPassRateSummaryFilter filter)
    {
        if (filter.StudentId <= 0)
        {
            throw new ArgumentException("Student ID must be greater than zero.", nameof(filter.StudentId));
        }

        return await connection.QueryFirstOrDefaultAsync<StudentPassRateSummaryItem>(ReportQueries.StudentPassRateSummary, filter);
    }
        

    public async Task<IEnumerable<DayOfWeekActivityItem>> GetDayOfWeekActivityAsync(DayOfWeekActivityFilter filter) =>
        await connection.QueryAsync<DayOfWeekActivityItem>(ReportQueries.DayOfWeekActivity, filter);
}