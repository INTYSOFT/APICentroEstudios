using System;
using System.Collections.Generic;

namespace api_intiSoft.Services.AnswerSheets;

public sealed class StudentAnswerSheet
{
    public string Dni { get; init; } = string.Empty;

    public IReadOnlyCollection<QuestionAnswer> Answers { get; init; } = Array.Empty<QuestionAnswer>();
}

public sealed class QuestionAnswer
{
    public int QuestionNumber { get; init; }

    public string Option { get; init; } = string.Empty;

    public double Confidence { get; init; }
}
