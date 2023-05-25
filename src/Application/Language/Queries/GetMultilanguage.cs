using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Language.Queries;

public record GetMultilanguage(int LanguageCulture) : IRequest<LanguageCulture>;


public class GetMultilanguageQueryHandler : IRequestHandler<GetMultilanguage, LanguageCulture>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMultilanguageQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LanguageCulture> Handle(GetMultilanguage request, CancellationToken cancellationToken)
    {
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LanguageDisplay, LanguageCulture>();
        });

        Mapper mapper = new Mapper(config);

        var languageInfo = (from cf in _context.DbSetLanguage
                               where cf.LanguageID == request.LanguageCulture
                               select new LanguageCulture
                               {
                                  LanguageCulture1 = cf.LanguageCulture,
                               }).FirstOrDefault();

        if (languageInfo == null)
        {
            return new LanguageCulture();
        }

        return languageInfo;
    }
}