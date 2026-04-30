using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Userselectedoption
{
    public int Id { get; set; }

    public int Userattemptanswerid { get; set; }

    public int Answerid { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual Userattemptanswer Userattemptanswer { get; set; } = null!;
}
