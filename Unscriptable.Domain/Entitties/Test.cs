using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Test
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Isrepeatable { get; set; }

    public string Type { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Publishedat { get; set; }

    public DateTime Deadline { get; set; }

    public int? Durationminutes { get; set; }

    public bool Ispublic { get; set; }

    public int? Passingscore { get; set; }

    public int? Maxattempts { get; set; }

    public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Testresult> Testresults { get; set; } = new List<Testresult>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Direction> Directions { get; set; } = new List<Direction>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
