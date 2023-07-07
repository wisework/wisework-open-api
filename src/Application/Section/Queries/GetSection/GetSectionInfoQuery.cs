using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;

namespace WW.Application.Section.Queries.GetSection;
public record GetSectionInfoQuery(int SectionId) : IRequest<SectionActiveList>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};

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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.SectionId <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("sectionID", "Section ID must be greater than 0"));

            throw new ValidationException(failures);
        }

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
                           where ppc.SectionInfoId == request.SectionId
                           select new SectionActiveList
                           {
                               SectionId = request.SectionId,
                               Code = ppc.Code,
                               Description = ppc.Description,
                               Status = ppc.Status

                           }).FirstOrDefault();

        if (sectionInfo == null)
        {
            throw new NotFoundException();
        }

        try
        {
            return sectionInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

      
    }

}