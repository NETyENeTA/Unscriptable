using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Student
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string Vkprofilelink { get; set; } = null!;

    public string? Avatarpath { get; set; }

    public int Userid { get; set; }

    public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    public virtual ICollection<Testresult> Testresults { get; set; } = new List<Testresult>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
