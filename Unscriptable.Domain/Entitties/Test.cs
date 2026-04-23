using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Domain.Enums;

namespace Unscriptable.Domain.Entitties;

public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TestType Type { get; set; }
    public int MaxScore { get; set; }

    public List<Attempt> Attempts { get; set; } = new();
}
