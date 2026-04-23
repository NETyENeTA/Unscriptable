using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class GroupTrendItem
{
    public DateTime Month { get; set; }
    public decimal AverageScore { get; set; }
    public int TestsCompleted { get; set; }
}
