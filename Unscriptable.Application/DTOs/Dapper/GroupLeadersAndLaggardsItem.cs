using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper;

public class GroupLeadersAndLaggardsItem
{
    public string GroupName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public decimal AvgScore { get; set; }
}