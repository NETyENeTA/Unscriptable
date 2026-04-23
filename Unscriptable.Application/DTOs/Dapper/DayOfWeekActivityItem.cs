using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class DayOfWeekActivityItem
{
    public int DayOfWeek { get; set; }
    public int TestsCompleted { get; set; }
    public int UniqueStudents { get; set; }
}
