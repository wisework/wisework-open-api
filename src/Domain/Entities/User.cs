using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class User
{
    public long UserId { get; set; }
    public int? ProfileImageId { get; set; }
    public int PositionId { get; set; }
    public int Version { get; set; }
    public string Status { get; set; } = null!;
    public int CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime? ChangePwdate { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Gender { get; set; } = null!;
    public string? Email { get; set; }
    public int TitleId { get; set; }
    public int? DefaultLanguageId { get; set; }
    public string? CitizenId { get; set; }
    public DateTime? WorkStart { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Tel { get; set; }
    public string? Guid { get; set; }
    public int? VerifyEmail { get; set; }
    public DateTimeOffset? LoginFailEnd { get; set; }
    public int? LoginFailCount { get; set; }
    public int? ForceChangePassword { get; set; }
}