using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Reports;

public class GroupLeadersAndLaggardsItem
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public string LeaderName { get; set; } = string.Empty;
    public int LeaderScore { get; set; }
    public string LaggardName { get; set; } = string.Empty;
    public int LaggardScore { get; set; }
}
