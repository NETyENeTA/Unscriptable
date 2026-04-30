using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Reports;

public class StudentPassRateItem
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public int TestsAvailable { get; set; }
    public int TestsPassed { get; set; }
    public decimal PassRate { get; set; } // 0-100
}

