using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Reports;

public class DayOfWeekActivityItem
{
    public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, ... 6=Saturday
    public string DayName { get; set; } = string.Empty; // "Пн", "Вт", ...
    public int TestsCompleted { get; set; } // Количество завершённых попыток
    public int UniqueStudents { get; set; } // Уникальные студенты (опционально)
}
