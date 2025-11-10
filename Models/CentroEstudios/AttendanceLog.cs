using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("attendance_logs", Schema = "academia")]
[Index("DeviceSn", "UserId", "PunchTime", Name = "ux_attendance_logs_sn_user_time", IsUnique = true)]
public partial class AttendanceLog
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("device_sn")]
    public string DeviceSn { get; set; } = null!;

    [Column("user_id")]
    public string UserId { get; set; } = null!;

    [Column("punch_time")]
    public DateTime PunchTime { get; set; }

    [Column("payload")]
    public string? Payload { get; set; }

    [Column("raw_line")]
    public string RawLine { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
