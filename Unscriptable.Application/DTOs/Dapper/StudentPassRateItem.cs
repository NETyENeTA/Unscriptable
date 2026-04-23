using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class StudentPassRateItem
{
    public string TestTitle { get; set; } = string.Empty;
    public int AttemptsTaken { get; set; }
    public string Status { get; set; } = string.Empty;
}
