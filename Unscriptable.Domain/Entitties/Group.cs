using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Domain.Entitties;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<Student> Students { get; set; } = new();
}
