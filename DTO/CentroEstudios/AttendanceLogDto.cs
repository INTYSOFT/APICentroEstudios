namespace api_intiSoft.DTO.CentroEstudios
{
    public class AttendanceLogDto
    {
        public long Id { get; init; }
        public string DeviceSn { get; init; } = default!;
        public string UserId { get; init; } = default!;
        public DateTime PunchTimeUtc { get; init; }
        public string? Payload { get; init; }
        public string RawLine { get; init; } = default!;
        public DateTime CreatedAtUtc { get; init; }
    }
}
