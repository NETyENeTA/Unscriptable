using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
