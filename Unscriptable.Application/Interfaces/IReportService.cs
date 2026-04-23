using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Application.DTOs.Dapper;
using Unscriptable.Application.DTOs.Filters;

namespace Unscriptable.Application.Interfaces;

public interface IReportService
{
    Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetGroupLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter filter);
    Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter filter);
    Task<IEnumerable<GroupTrendItem>> GetGroupTrendAsync(GroupTrendFilter filter);
    Task<IEnumerable<StudentRatingItem>> GetStudentRatingAsync(StudentRatingFilter filter);
    Task<IEnumerable<StudentMonthlyProgressItem>> GetStudentMonthlyProgressAsync(StudentMonthlyProgressFilter filter);
    Task<IEnumerable<StudentPassRateItem>> GetStudentPassRateAsync(StudentPassRateFilter filter);
    Task<StudentPassRateSummaryItem?> GetStudentPassRateSummaryAsync(StudentPassRateSummaryFilter filter);
    Task<IEnumerable<DayOfWeekActivityItem>> GetDayOfWeekActivityAsync(DayOfWeekActivityFilter filter);
}