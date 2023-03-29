using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;

namespace WW.Application.CompanyProfile.Queies.GetCompany;
public record GetCompanyQuery : IRequest<Company>;

public class GetCompanyQueryHandler : RequestHandler<GetCompanyQuery, Company>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override Company Handle(GetCompanyQuery request)
    {
        throw new NotImplementedException();
    }

    /*protected override Company Handle(GetCompanyQuery request)
    {
        // todo: get company id from token
       // return (Company)_context.DbSetCompanies.Select(company => company.CompanyId == 1).ProjectTo<Company>(_mapper.ConfigurationProvider);
    }*/
}
