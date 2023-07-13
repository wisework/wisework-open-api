using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Application.GeneralConsents.Commands.GeneralConsentLastId;
using WW.Application.GeneralConsents.Queries;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.GeneralConsents.Commands.GeneralConsentFilterQuery;
public record GeneralConsentFilterQueryCommand : IRequest<PaginatedList<GeneralConsent>>
{
    public Domain.Common.PaginationParams? PaginationParams { get; set; }
    public Domain.Common.SortingParams SortingParams { get; set; }
    public Domain.Common.GeneralConsentFilterKey? GeneralConsentFilterKey { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GeneralConsentFilterQueryCommandHandler : IRequestHandler<GeneralConsentFilterQueryCommand, PaginatedList<GeneralConsent>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GeneralConsentFilterQueryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GeneralConsent>> Handle(GeneralConsentFilterQueryCommand request, CancellationToken cancellationToken)
    {

        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
       
        try
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Consent_Consent, GeneralConsent>()
                .ForMember(d => d.ConsentId, a => a.MapFrom(s => s.ConsentId))
                .ForMember(d => d.FullName, a => a.MapFrom(s => s.FullName))
                .ForMember(d => d.IdCardNumber, a => a.MapFrom(s => s.IdCardNumber))
                .ForMember(d => d.PhoneNumber, a => a.MapFrom(s => s.PhoneNumber));
            });

            Mapper mapper = new Mapper(config);
            //todo:edit conpanyid หลังมีการทำ identity server
            var query = _context.DbSetConsent.Where(consent => consent.CompanyId == request.authentication.CompanyID && consent.New == 1 && consent.Status != "X");
            if ( query == null )
            {
                throw new NotFoundException();
            }    

            #region Sorting 
            if (request.SortingParams != null)
            {
                List<string> listColumName = new List<string>(new string[] {
                                        "CompanyID",
                                        "CollectionPointID",
                                        "ConsentDatetime",
                                        "WebsiteID",
                                        "Email",
                                        "NameSurname",
                                        "Tel",
                                        "Createby",
                                        "CreateDate",
                                        "FromBrowser",
                                        "FromWebsite",
                                        "ConsentSignature",
                                        "VerifyType",
                                        "TotalTransactions",
                                        "New",
                                        "CardNumber",
                                        "Remark",
                                        "EventCode",
                                        "Expired",
                                        "HasNotificationRenew",
                                        "UID",
                                        "AgeRange",
                                        "Status",
                                        "UpdateBy",
                                        "UpdateDate"
                                    });

                /*SortDesc == 1 คือ Desc 0 คือ ASC*/

                string sortColumn = request.SortingParams.SortName;
                var haveColumName = listColumName.Contains(sortColumn);
                if (!haveColumName)
                {
                    throw new Exception("Parameter sort name is unavailable.");
                }

                if (request.SortingParams.SortDesc == 1)
                {
                    query = query.OrderByDescending(p => EF.Property<object>(p, sortColumn));
                }
                else
                {
                    query = query.OrderBy(p => EF.Property<object>(p, sortColumn));
                }

            }
            else
            {
                query = query.OrderBy(p => EF.Property<object>(p, "ConsentID"));
            }


            #endregion

            #region Filter consent primary key
            if (request.GeneralConsentFilterKey != null)
            {
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.IdCardNumber))
                {
                    query = query.Where(p => p.IdCardNumber == request.GeneralConsentFilterKey.IdCardNumber);

                    if (query.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("idCardNumber", "No matching ID card number found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.FullName))
                {
                    query = query.Where(p => p.FullName == request.GeneralConsentFilterKey.FullName);

                    if (query.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("fullName", "No matching full name found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.PhoneNumber))
                {
                    query = query.Where(p => p.PhoneNumber == request.GeneralConsentFilterKey.PhoneNumber);

                    if (query.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("phoneNumber", "No matching phone number found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.Email))
                {
                    query = query.Where(p => p.Email == request.GeneralConsentFilterKey.Email);

                    if (query.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("email", "No matching email found"));

                        throw new ValidationException(failures);
                    }
                }
                if (request.GeneralConsentFilterKey.Uid != null)
                {
                    query = query.Where(p => p.Uid == request.GeneralConsentFilterKey.Uid);
                }
                if (request.GeneralConsentFilterKey.StartDate != null)
                {
                    DateTimeOffset startDateVal = (DateTimeOffset)request.GeneralConsentFilterKey.StartDate;
                    query = query.Where(p => p.ConsentDatetime >= startDateVal);
                }
                if (request.GeneralConsentFilterKey.EndDate != null)
                {
                    DateTimeOffset endDateVal = (DateTimeOffset)request.GeneralConsentFilterKey.EndDate;
                    query = query.Where(p => p.ConsentDatetime <= endDateVal);
                }
            }
            #endregion

            var Offset = 1;
            var Limit = 10;
            if (request.PaginationParams.Offset.Value <= 0 || request.PaginationParams.Limit.Value <= 0)
            {
                List<ValidationFailure> failures = new List<ValidationFailure> { };

                if (request.PaginationParams.Offset.Value <= 0)
                {
                    failures.Add(new ValidationFailure("offset", "Offset must be greater than 0"));
                }
                if (request.PaginationParams.Limit.Value <= 0)
                {
                    failures.Add(new ValidationFailure("limit", "Limit must be greater than 0"));
                }

                throw new ValidationException(failures);
            }
            if (request.PaginationParams == null)
            {
                Offset = request.PaginationParams.Offset.Value;
                Limit = request.PaginationParams.Limit.Value;
            }


            PaginatedList<GeneralConsent> model = await query
                .ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(Offset, Limit);

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
                item.TotalTransactions = (int)(long)item.TotalTransactions;
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
                //item.TotalCount = (int)item.TotalCount;
                item.CompanyId = (int)item.CompanyId;
                item.CompanyName = companyList.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CompanyName).FirstOrDefault();
                item.Status = (string)item.Status;
                item.VerifyType = (string)item.VerifyType;
            }

            return model;
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (NotFoundException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

    }
}
