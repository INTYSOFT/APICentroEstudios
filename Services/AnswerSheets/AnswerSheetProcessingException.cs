using System;

namespace api_intiSoft.Services.AnswerSheets;

public sealed class AnswerSheetProcessingException : Exception
{
    public AnswerSheetProcessingException(string message)
        : base(message)
    {
    }

    public AnswerSheetProcessingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
