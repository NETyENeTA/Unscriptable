using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Filters;

public class GroupLeadersAndLaggardsFilter
{
    public int? DirectionId { get; set; }
    public int? CourseId { get; set; }
}
