﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WW.Application.GeneralConsents.Queries;
public record GeneralConsentQuery : IRequest<PaginatedList<GeneralConsent>>
{
    public int Offset { get; init; } = 1;
    public int Limit { get; init; } = 10;
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GeneralConsentQueryHandler : IRequestHandler<GeneralConsentQuery, PaginatedList<GeneralConsent>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GeneralConsentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GeneralConsent>> Handle(GeneralConsentQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        if (request.Offset <= 0 || request.Limit <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };

            if (request.Offset <= 0)
            {
                failures.Add(new ValidationFailure("offset", "Offset must be greater than 0"));
            }
            if (request.Limit <= 0)
            {
                failures.Add(new ValidationFailure("limit", "Limit must be greater than 0"));
            }

            throw new ValidationException(failures);
        }

        try
        {
            // Configure AutoMapper mappings
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Consent_Consent, GeneralConsent>()
                    .ForMember(d => d.ConsentId, a => a.MapFrom(s => s.ConsentId));
                config.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));

            });

            Mapper mapper = new Mapper(mapperConfig);

            //todo:edit conpanyid หลังมีการทำ identity server
            PaginatedList<GeneralConsent> model =
                await _context.DbSetConsent.Where(p => p.CompanyId == request.authentication.CompanyID && p.Status == Status.Active.ToString())
                .ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(request.Offset, request.Limit);

            var collectionpoints = _context.DbSetConsentCollectionPoints.Where(cp => cp.CompanyId == request.authentication.CompanyID && cp.Status != Status.X.ToString()).ToList();
            var companies = _context.DbSetCompanies.Where(c => c.Status == "A").ToList();
            var webSites = _context.DbSetConsentWebsite.Where(ws => ws.CompanyId == request.authentication.CompanyID && ws.Status != Status.X.ToString()).ToList();

            #region fetch collectionpoint
            var collectionPointIds = model.Items.Select(c => c.CollectionPointId);
            var collectionpointList = (from cp in collectionpoints
                                       join c in companies on cp.CompanyId equals (int)c.CompanyId
                                       join ws in webSites on cp.WebsiteId equals ws.WebsiteId
                                       where collectionPointIds.Contains(cp.CollectionPointId)
                                       select new KeyValuePair<int, CollectionPointInfo>(cp.CollectionPointId,
                                       new CollectionPointInfo
                                       {
                                           CollectionPointId = cp.CollectionPointId,
                                           Guid = new Guid(cp.Guid),
                                           CompanyId = (int)c.CompanyId,
                                           Version = cp.Version,
                                           Status = cp.Status,
                                       })).ToList();
            #endregion

            #region fetch company
            var companyList = (from cp in collectionpoints
                               join c in companies on cp.CompanyId equals (int)c.CompanyId
                               where collectionPointIds.Contains(cp.CollectionPointId)
                               select new KeyValuePair<int, Company>(cp.CollectionPointId, new Company
                               {
                                   CompanyId = c.CompanyId,
                                   CompanyName = c.Name,
                                   Status = c.Status
                               })).ToList();
            var companyLookup = companyList.ToLookup(item => item.Key, item => item.Value);
            #endregion

            #region fetch purpose
            var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
                               join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                               where cp.Status == Status.Active.ToString()
                               && p.Status == Status.Active.ToString()
                               && collectionPointIds.Contains(cp.CollectionPointId)
                               select new KeyValuePair<int, GeneralConsentPurpose>(cp.CollectionPointId,
                                   new GeneralConsentPurpose
                                   {
                                       PurposeId = p.PurposeId,
                                       CompanyId = p.CompanyId,
                                       Code = p.Code,
                                       Description = p.Description,
                                       WarningDescription = p.WarningDescription,
                                       PurposeCategoryId = p.PurposeCategoryId,
                                       ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
                                       Guid = new Guid(p.Guid),
                                       Version = p.Version,
                                       Priority = cp.Priority,
                                       Status = p.Status
                                   })).ToList();

            var purposeLookup = purposeList.ToLookup(item => item.Key, item => item.Value);
            #endregion

            #region fetch website
            var websiteList = (from cp in _context.DbSetConsentCollectionPoints
                               join w in _context.DbSetConsentWebsite on cp.WebsiteId equals w.WebsiteId
                               where collectionPointIds.Contains(cp.CollectionPointId)
                               select new KeyValuePair<int, GeneralConsentWebsiteObject>(cp.CollectionPointId,
                                   new GeneralConsentWebsiteObject
                                   {
                                       Id = w.WebsiteId,
                                       Description = w.Description,
                                       UrlHomePage = w.Url,
                                       UrlPolicyPage = w.Urlpolicy,
                                   })).ToList();

            var websiteLookup = websiteList.ToLookup(item => item.Key, item => item.Value);
            #endregion

            foreach (var item in model.Items)
            {
                item.CompanyId = (int)(long)item.CompanyId;
                item.CollectionPointId = (int)(long)item.CollectionPointId;
                item.Uid = (int)(long)item.Uid;
                item.FullName = (string)item.FullName;
                item.GeneralConsentWebsiteObject = websiteLookup[item.CollectionPointId.Value].FirstOrDefault();  //JSON
                item.CollectionPointGuid = collectionpointList.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Guid.ToString()).FirstOrDefault();
                item.CollectionPointVersion = collectionpointList.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Version).FirstOrDefault();
                item.PurposeList = purposeLookup[item.CollectionPointId.Value].ToList(); // JSON 
                item.FromBrowser = (string)item.FromBrowser;
                item.PhoneNumber = (string)item.PhoneNumber;
                item.IdCardNumber = (string)item.IdCardNumber;
                item.Email = (string)item.Email;
                item.Remark = (string)item.Remark;
                item.CompanyName = companyList.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CompanyName).FirstOrDefault();
                item.Status = (string)item.Status;
                item.VerifyType = (string)item.VerifyType;
            }

            return model;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
       
    }
    
}

