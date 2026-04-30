using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Answer
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool Iscorrect { get; set; }

    public int Questionid { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Userselectedoption> Userselectedoptions { get; set; } = new List<Userselectedoption>();
}
