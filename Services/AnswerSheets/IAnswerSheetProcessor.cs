using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace api_intiSoft.Services.AnswerSheets;

public interface IAnswerSheetProcessor
{
    Task<IReadOnlyCollection<StudentAnswerSheet>> ProcessAsync(Stream pdfStream, CancellationToken cancellationToken);
}
