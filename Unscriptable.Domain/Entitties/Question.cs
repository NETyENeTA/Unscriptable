using System;
using System.Collections.Generic;

namespace Unscriptable.Domain.Entitties;

public partial class Question
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int Number { get; set; }

    public string Description { get; set; } = null!;

    public string Answertype { get; set; } = null!;

    public bool Isscoring { get; set; }

    public int? Maxscore { get; set; }

    public int Testid { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<Userattemptanswer> Userattemptanswers { get; set; } = new List<Userattemptanswer>();
}
