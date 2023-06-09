﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;

namespace WW.Application.Language.Queries;
public record GetLanguageQuery(string LanguageCulturesKey,string ResourceKeys) : IRequest<Dictionary<string, Dictionary<string, string>>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
};

public class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, Dictionary<string, Dictionary<string, string>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public GetLanguageQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<Dictionary<string, Dictionary<string, string>>> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
    {

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        var languageCultures = request.LanguageCulturesKey.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(lang => lang.Trim());

        var resourceKeys = request.ResourceKeys?.Split(',', StringSplitOptions.RemoveEmptyEntries)?.Select(key => key.Trim()) ?? Enumerable.Empty<string>();

        var query = from lc in _context.DbSetLanguage
                    join lsr in _context.DbSetLocalStringResource on lc.LanguageCulture equals lsr.LanguageCulture
                    where languageCultures.Contains(lc.LanguageCulture)
                    select new { lc.LanguageCulture, lsr.ResourceKey, lsr.ResourceValue };



        if (resourceKeys != null && resourceKeys.Any())
            query = query.Where(item => resourceKeys.Contains(item.ResourceKey));

        var result = await query.ToListAsync();

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

        if (languageDictionary.Count == 0)
        {
            throw new NotFoundException();
        }

        try
        {         
            return languageDictionary;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

        

    }
}
