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
using WW.Domain.Enums;

namespace WW.Application.Purpose.Queries.GetPurpose;
public record GetPurposeInfoQuery(int id) : IRequest<PurposeActiveList>
{

    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};

public class GetPurposeInfoQueryHandler : IRequestHandler<GetPurposeInfoQuery, PurposeActiveList>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurposeInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurposeActiveList> Handle(GetPurposeInfoQuery request, CancellationToken cancellationToken)
    {

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.id <= 0 )
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            if (request.id <= 0)
            {
                failures.Add(new ValidationFailure("ID", "ID must be greater than 0"));
            }
            

            throw new ValidationException(failures);
        }

        try
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Consent_Purpose, PurposeActiveList>();
                cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            });

            Mapper mapper = new Mapper(config);

            var purposeInfo = (from cf in _context.DbSetConsentPurpose
                               where cf.PurposeId == request.id && cf.CompanyId == 1 && cf.Status != Status.X.ToString()
                               select new PurposeActiveList
                               {
                                   PurposeID = cf.PurposeId,
                                   GUID = new Guid(cf.Guid),
                                   PurposeType = cf.PurposeType,
                                   CategoryID = cf.CategoryID,
                                   Code = cf.Code,
                                   Description = cf.Description,
                                   KeepAliveData = cf.KeepAliveData,
                                   LinkMoreDetail = cf.LinkMoreDetail,
                                   Status = cf.Status,
                                   TextMoreDetail = cf.TextMoreDetail,
                                   WarningDescription = cf.WarningDescription,
                                   ExpiredDateTime = cf.ExpiredDateTime,
                                   Language = cf.Language,


                               }).FirstOrDefault();

            if (purposeInfo == null)
            {
                return new PurposeActiveList();
            }
            return purposeInfo;
        }
        catch (Exception ex)
        {
         
            throw new InternalServerException(ex.Message);
        }
    }
      
    }

