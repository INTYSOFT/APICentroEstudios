namespace api_intiSoft.DTO.CentroEstudios
{
    public class AttendanceLogCreateDto
    {
        public string DeviceSn { get; init; } = default!;
        public string UserId { get; init; } = default!;
        public DateTime PunchTimeUtc { get; init; } // UTC
        public string? Payload { get; init; }
        public string RawLine { get; init; } = default!;
    }
}
