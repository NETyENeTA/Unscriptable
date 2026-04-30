using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Attempt
{
    public int Id { get; set; }

    public DateTime Startedat { get; set; }

    public DateTime? Submittedat { get; set; }

    public int? Score { get; set; }

    public int Testid { get; set; }

    public int Studentid { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<Testresult> Testresults { get; set; } = new List<Testresult>();

    public virtual ICollection<Userattemptanswer> Userattemptanswers { get; set; } = new List<Userattemptanswer>();
}
