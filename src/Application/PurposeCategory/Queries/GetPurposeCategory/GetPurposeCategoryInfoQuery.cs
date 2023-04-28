using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.PurposeCategory.Queries.GetPurposeCategory;
public record GetPurposeCategoryInfoQuery(int Id) : IRequest<PurposeCategoryActiveList>;

public class GetPurposeCategoryInfoQueryHandler : IRequestHandler<GetPurposeCategoryInfoQuery, PurposeCategoryActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurposeCategoryInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurposeCategoryActiveList> Handle(GetPurposeCategoryInfoQuery request, CancellationToken cancellationToken)
    {

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_PurposeCategory, PurposeCategoryActiveList>();
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        var purposecategoryInfo = (from ppc in _context.DbSetConsentPurposeCategory
                                   join uCreate in _context.DbSetUser on ppc.CreateBy equals uCreate.CreateBy
                                   join uUpdate in _context.DbSetUser on ppc.CreateBy equals uUpdate.UpdateBy
                                   join c in _context.DbSetCompanies on ppc.CompanyID equals (int)(long)c.CompanyId
                                   join w in _context.DbSetConsentWebsite on (int)(long)c.CompanyId equals w.CompanyId
                                   where ppc.ID == request.Id 
                                   select new PurposeCategoryActiveList
                                   {
                                      Code = ppc.Code,
                                      Description = ppc.Description,
                                      Language = ppc.Language,
                                      Status = ppc.Status
                                      
                                   }).FirstOrDefault();

        return purposecategoryInfo;
    }

}