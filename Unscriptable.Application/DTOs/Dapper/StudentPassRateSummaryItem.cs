using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class StudentPassRateSummaryItem
{
    public string FullName { get; set; } = string.Empty;
    public int TestsAttempted { get; set; }
    public int TestsPassed { get; set; }
    public int TotalScore { get; set; }
    public decimal AverageScore { get; set; }
}
