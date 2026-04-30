using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Application.DTOs.Dapper.Reports;
using Unscriptable.Application.DTOs.Dapper.Filters;

namespace Unscriptable.Application.Interfaces;

public interface IReportsService
{
    Task<IEnumerable<DayOfWeekActivityItem>> GetDayOfWeekActivityAsync(DayOfWeekActivityFilter f);
    Task<IEnumerable<GroupLeadersAndLaggardsItem>> GetLeadersAndLaggardsAsync(GroupLeadersAndLaggardsFilter f);
    Task<IEnumerable<GroupTrendItem>> GetGroupTrendAsync(GroupTrendFilter f);
    Task<IEnumerable<StudentMonthlyProgressItem>> GetStudentMonthlyProgressAsync(StudentMonthlyProgressFilter f);
    Task<IEnumerable<StudentPassRateItem>> GetStudentPassRateAsync(StudentPassRateFilter f);
    Task<StudentPassRateSummaryItem?> GetStudentPassRateSummaryAsync(StudentPassRateSummaryFilter f);
    Task<IEnumerable<StudentRatingItem>> GetStudentRatingAsync(StudentRatingFilter f);
    Task<IEnumerable<StudentTestResultsItem>> GetStudentTestResultsAsync(StudentTestResultsFilter f);
}