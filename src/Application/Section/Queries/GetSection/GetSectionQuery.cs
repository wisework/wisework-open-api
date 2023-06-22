using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Section.Queries.GetSection;

public record GetSectionQuery : IRequest<PaginatedList<SectionActiveList>>
{
    public int offset { get; init; } = 1;
    public int limit { get; init; } = 10;
}
public class GetSectionQueryHandler : IRequestHandler<GetSectionQuery, PaginatedList<SectionActiveList>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetSectionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<SectionActiveList>> Handle(GetSectionQuery request, CancellationToken cancellationToken)
    {
            
        try
        {
            
                if (request.offset < 0 || request.limit < 0)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure> { };

                    if (request.offset < 0)
                    {
                        failures.Add(new ValidationFailure("Offset", "Offset must be greater than 0"));
                    }
                    if (request.limit < 0)
                    {
                        failures.Add(new ValidationFailure("Limit", "Limit must be greater than 0"));
                    }

                    throw new ValidationException(failures);
                }
            

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Consent_SectionInfo, SectionActiveList>().ForMember(d => d.SectionId, a => a.MapFrom(s => s.SectionInfoId));
            });

            Mapper mapper = new Mapper(config);

            //todo:edit conpanyid หลังมีการทำ identity server
            PaginatedList<SectionActiveList> model =
                await _context.DbSetConsentSectionInfo.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
                .ProjectTo<SectionActiveList>(mapper.ConfigurationProvider).PaginatedListAsync(request.offset, request.limit);

            
            if (model.Items == null || model.Items.Count == 0)
            {
                throw new NotFoundException("User not found"); // 404 Not Found
            }

            return model;
        }
        catch (NotFoundException)
        {
            throw new NotFoundException("User not found"); // 404 Not Found
        }
        catch (ValidationException)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            if (request.offset < 0 || request.limit < 0)
            {
               

                if (request.offset < 0)
                {
                    failures.Add(new ValidationFailure("Offset", "Offset must be greater than 0"));
                }
                if (request.limit < 0)
                {
                    failures.Add(new ValidationFailure("Limit", "Limit must be greater than 0"));
                }

               
            }

            throw new ValidationException(failures);
        }

        catch (Exception ex)
        {
            throw new InternalException("An internal server error occurred."); // 500 error
        }
    }
}