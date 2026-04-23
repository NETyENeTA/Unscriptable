using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class StudentMonthlyProgressItem
{
    public DateTime Month { get; set; }
    public decimal AvgScore { get; set; }
    public int MaxScore { get; set; }
}
