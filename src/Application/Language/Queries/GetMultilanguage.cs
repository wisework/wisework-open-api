using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Language.Queries;


public record GetMultilanguage(String Key) : IRequest<Dictionary<string, Dictionary<string, string>>>;

public class GetMultilanguageHandler : IRequestHandler<GetMultilanguage, Dictionary<string, Dictionary<string, string>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public GetMultilanguageHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<Dictionary<string, Dictionary<string, string>>> Handle(GetMultilanguage request, CancellationToken cancellationToken)
    {
        var result = await (from lc in _context.DbSetLanguage
                            join lsr in _context.DbSetLocalStringResource on lc.LanguageCulture equals lsr.LanguageCulture
                            where lsr.LanguageCulture == request.Key
                            select new { lc.LanguageCulture, lsr.ResourceKey, lsr.ResourceValue })
                            .ToListAsync();

        var languageDictionary = new Dictionary<string, Dictionary<string, string>>();

        foreach (var item in result)
        {
            if (!languageDictionary.TryGetValue(item.LanguageCulture, out var resourceDictionary))
            {
                resourceDictionary = new Dictionary<string, string>();
                languageDictionary[item.LanguageCulture] = resourceDictionary;
            }

            resourceDictionary[item.ResourceKey] = item.ResourceValue;
        }

        return languageDictionary;
    }
}