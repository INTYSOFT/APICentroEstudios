using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("attendance_daily", Schema = "academia")]
[Index(nameof(LocalDate), Name = "idx_attdaily_date")]
[Index(nameof(UserId), nameof(LocalDate), Name = "idx_attdaily_user_date")]
[Index(nameof(DeviceSn), nameof(LocalDate), Name = "idx_attdaily_sn_date")]
[Index(nameof(DeviceSn), nameof(UserId), nameof(PunchTimeUtc), IsUnique = true, Name = "ux_attendance_daily")]
public partial class AttendanceDaily
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("local_date")]
    public DateOnly LocalDate { get; set; }

    [Column("device_sn")]
    public string DeviceSn { get; set; } = null!;

    [Column("user_id")]
    public string UserId { get; set; } = null!;

    [Column("punch_time_utc")]
    public DateTime PunchTimeUtc { get; set; }           // Kind=Utc

    [Column("punch_time_local", TypeName = "timestamp without time zone")]
    public DateTime PunchTimeLocal { get; set; }         // Unspecified (hora local)

    [Column("payload")]
    public string? Payload { get; set; }

    [Column("raw_line")]
    public string RawLine { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }              // Utc
}
