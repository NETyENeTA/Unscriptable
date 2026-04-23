using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Filters;

public class DayOfWeekActivityFilter
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
