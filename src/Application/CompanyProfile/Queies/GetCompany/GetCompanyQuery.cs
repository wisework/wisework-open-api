using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WW.Application.CompanyProfile.Queies.GetCompany;
public record GetCompanyQuery(int id) : IRequest<Company>;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, Company>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Company> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Companies, Company>();
        });

        Mapper mapper = new Mapper(config);

        var companyInfo = (from cf in _context.DbSetCompanies
                               where cf.CompanyId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString()
                               select new Company
                               {
                                  CompanyId = cf.CompanyId,
                                   CompanyName =cf.Name,
                                   LogoImage = cf.LogoImage,
                                   Status = cf.Status,
                                   CreateBy = cf.CreateBy.ToString(),
                                   CreateDate= cf.CreateDate.ToString(),
                                   UpdateBy= cf.UpdateBy.ToString(),
                                   UpdateDate= cf.UpdateDate.ToString(),
                               }).FirstOrDefault();

        if (companyInfo == null)
        {
            return new Company();
        }
        return companyInfo;
    }
}
