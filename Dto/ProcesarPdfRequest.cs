using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace api_intiSoft.Dto
{
    public class ProcesarPdfRequest
    {
        [Required]
        public IFormFile Pdf { get; set; } = default!;

        // Estos dos pueden venir como campos del form-data
        [Required]
        public int EvaluacionProgramadaId { get; set; }

        [Required]
        public int SeccionId { get; set; }

    }
}
