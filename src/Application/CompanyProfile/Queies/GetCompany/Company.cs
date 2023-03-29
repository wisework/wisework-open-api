using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WW.Application.Common.Mappings;
using WW.Domain.Entities;

namespace WW.Application.CompanyProfile.Queies.GetCompany;
public class Company : IMapFrom<Companies>
{
    public long CompanyId { get; set; }
    public string Status { get; set; } = null!;
    public int? CompanyGroupId { get; set; }
    public int? CompanyTypeId { get; set; }
    public int? CompanyStatusId { get; set; }
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; }
    public string? RefreshToken { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Company, Companies>()
            .ForMember(d => d.CompanyId, opt => opt.MapFrom(s => s.CompanyId));
    }

}
