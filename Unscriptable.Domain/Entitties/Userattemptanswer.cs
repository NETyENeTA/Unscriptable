using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Userattemptanswer
{
    public int Id { get; set; }

    public bool Iscorrect { get; set; }

    public int Scoreawarded { get; set; }

    public int Attemptid { get; set; }

    public int Questionid { get; set; }

    public virtual Attempt Attempt { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Userselectedoption> Userselectedoptions { get; set; } = new List<Userselectedoption>();

    public virtual Usertextanswer? Usertextanswer { get; set; }
}
