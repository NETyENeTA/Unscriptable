using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Filters;

public class DayOfWeekActivityFilter
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? GroupId { get; set; } // Опциональный фильтр по группе
}
