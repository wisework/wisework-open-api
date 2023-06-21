using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;

namespace WW.Application.Section.Queries.GetSection;
public record GetSectionInfoQuery(int Id) : IRequest<SectionActiveList>;

public class GetSectionInfoQueryHandler : IRequestHandler<GetSectionInfoQuery, SectionActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSectionInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SectionActiveList> Handle(GetSectionInfoQuery request, CancellationToken cancellationToken)
    {

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_SectionInfo, SectionActiveList>();
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        var sectionInfo = (from ppc in _context.DbSetConsentSectionInfo
                                   join uCreate in _context.DbSetUser on ppc.CreateBy equals uCreate.CreateBy
                                   join uUpdate in _context.DbSetUser on ppc.CreateBy equals uUpdate.UpdateBy
                                   join c in _context.DbSetCompanies on ppc.CompanyId equals (int)(long)c.CompanyId
                                   join w in _context.DbSetConsentWebsite on (int)(long)c.CompanyId equals w.CompanyId
                                   where ppc.SectionInfoId == request.Id
                                   select new SectionActiveList
                                   {
                                       SectionId = request.Id,
                                       Code = ppc.Code,
                                       Description = ppc.Description,
                                       Status = ppc.Status

                                   }).FirstOrDefault();

        return sectionInfo;
    }

}