using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Testresult
{
    public int Id { get; set; }

    public bool Passed { get; set; }

    public int Testid { get; set; }

    public int Attemptid { get; set; }

    public int Studentid { get; set; }

    public virtual Attempt Attempt { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
