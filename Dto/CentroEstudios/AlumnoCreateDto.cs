namespace api_intiSoft.Dto.CentroEstudios
{
    public sealed class AlumnoCreateDto
    {
        public string Dni { get; set; } = default!;
        public string? Apellidos { get; set; }
        public string? Nombres { get; set; }
        public string? FechaNacimiento { get; set; } // llega como string/ISO
        public string? Celular { get; set; }
        public string? Correo { get; set; }
        //UbigeoCode
        public string? UbigeoCode { get; set; }
        public string? Direccion { get; set; }
        //ColegioId
        public int? ColegioId { get; set; }
        //observcaiones
        public string? Observacion { get; set; }
        public bool Activo { get; set; } = true;
        //FotoUrl
        public string? FotoUrl { get; set; }
    }

}
