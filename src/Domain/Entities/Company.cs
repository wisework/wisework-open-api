using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Companies
{
    [Key]
    public long CompanyId { get; set; }
    public int? ProfileImageId { get; set; }
    public int Version { get; set; }
    public string Status { get; set; } = null!;
    public int CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? TaxNo { get; set; }
    public int? CompanyGroupId { get; set; }
    public int? CompanyTypeId { get; set; }
    public int? CompanyStatusId { get; set; }
    public int? SalePerson { get; set; }
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; }
    public int? ExpireDate { get; set; }
    public string? RefreshToken { get; set; }
    public int? TrialVersion { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? AmountUser { get; set; }
    public int? TrialStatusId { get; set; }
    public int? CountConcurrent { get; set; }
    public string? DsrToken { get; set; }
}
