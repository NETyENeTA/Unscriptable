using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Usertextanswer
{
    public int Id { get; set; }

    public string Textanswer { get; set; } = null!;

    public int Userattemptanswerid { get; set; }

    public virtual Userattemptanswer Userattemptanswer { get; set; } = null!;
}
