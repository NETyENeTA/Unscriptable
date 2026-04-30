using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Reports;

public class StudentRatingItem
{
    public int Rank { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public int TotalScore { get; set; }
}
