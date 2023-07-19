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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

namespace WW.Application.GeneralConsents.Commands.GeneralConsentFilter;
public record GeneralConsentFilterCommand : IRequest<List<GeneralConsent>>
{
    public Domain.Common.PaginationParams PaginationParams { get; set; }
    public Domain.Common.SortingParams? SortingParams { get; set; }
    public Domain.Common.GeneralConsentFilterKey? GeneralConsentFilterKey { get; set; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GeneralConsentFilterCommandHandler : IRequestHandler<GeneralConsentFilterCommand, List<GeneralConsent>>
{
    private readonly IApplicationDbContext _context;

    public GeneralConsentFilterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GeneralConsent>> Handle(GeneralConsentFilterCommand request, CancellationToken cancellationToken)
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
            if (request.PaginationParams.Offset <= 0 || request.PaginationParams.Limit <= 0)
            {
                List<ValidationFailure> failures = new List<ValidationFailure> { };

                if (request.PaginationParams.Offset <= 0)
                {
                    failures.Add(new ValidationFailure("offset", "Offset must be greater than 0"));
                }
                if (request.PaginationParams.Limit <= 0)
                {
                    failures.Add(new ValidationFailure("limit", "Limit must be greater than 0"));
                }

                throw new ValidationException(failures);
            }
            PaginatedList<GeneralConsent> model = await _context.DbSetConsent
                .ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(request.PaginationParams.Offset, request.PaginationParams.Limit);

            var query = model.Items;

            var q = query;
            var queryTemp = (from c in query
                             join cp in _context.DbSetConsentCollectionPoints on c.CollectionPointId equals cp.CollectionPointId
                             join cpt in _context.DbSetConsentCollectionPointItem on cp.CollectionPointId equals cpt.CollectionPointId
                             join cpp in _context.DbSetConsentPurpose on cpt.PurposeId equals cpp.PurposeId
                             join cy in _context.DbSetCompanies on c.CompanyId equals (int)cy.CompanyId
                             where c.Status == "Active" && c.CompanyId == request.authentication.CompanyID
                                   && cp.Status == "Active" && cpt.Status == "Active" && cpp.Status == "Active"
                                   && cy.Status == "A"
                             select new GeneralConsent
                                       {
                                           ConsentId = c.ConsentId,
                                           CollectionPointId = cp.CollectionPointId,
                                           Uid = c.Uid,
                                           TotalTransactions = c.TotalTransactions,
                                           FullName = c.FullName,
                                           CollectionPointGuid = cp.Guid,
                                           ConsentDateTime = c.ConsentDateTime,
                                           CollectionPointVersion = cp.Version,
                                           FromBrowser = c.FromBrowser,
                                           PhoneNumber = c.PhoneNumber,
                                           IdCardNumber = c.IdCardNumber,
                                           Email = c.Email,
                                           Remark = c.Remark,
                                           CompanyId = c.CompanyId,
                                           CompanyName = cy.Name,
                                           Status = c.Status,
                                           VerifyType = c.VerifyType,
                                           
                                       }).ToList();
            var collectionPointIds = queryTemp.Select(c => c.CollectionPointId);

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

            #endregion
            #region Sorting 
            if (request.SortingParams != null)
            {
                List<string> listColumName = new List<string>(new string[] {
                                        "ConsentID",
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
                    //queryTemp = queryTemp.OrderByDescending(p => EF.Property<object>(p, sortColumn));
                    switch (sortColumn)
                    {
                        case "ConsentID":
                            queryTemp = queryTemp.OrderByDescending(x => x.ConsentId).ToList();
                            break;
                        case "CollectionPointID":
                            queryTemp = queryTemp.OrderByDescending(x => x.CollectionPointId).ToList();
                            break;
                        case "ConsentDatetime":
                            queryTemp = queryTemp.OrderByDescending(x => x.ConsentDateTime).ToList();
                            break;
                        case "Email":
                            queryTemp = queryTemp.OrderByDescending(x => x.Email).ToList();
                            break;
                        case "NameSurname":
                            queryTemp = queryTemp.OrderByDescending(x => x.FullName).ToList();
                            break;
                        case "Tel":
                            queryTemp = queryTemp.OrderByDescending(x => x.PhoneNumber).ToList();
                            break;
                        case "FromBrowser":
                            queryTemp = queryTemp.OrderByDescending(x => x.FromBrowser).ToList();
                            break;
                        case "VerifyType":
                            queryTemp = queryTemp.OrderByDescending(x => x.VerifyType).ToList();
                            break;
                        case "TotalTransactions":
                            queryTemp = queryTemp.OrderByDescending(x => x.TotalTransactions).ToList();
                            break;
                        case "CardNumber":
                            queryTemp = queryTemp.OrderByDescending(x => x.IdCardNumber).ToList();
                            break;
                        case "Remark":
                            queryTemp = queryTemp.OrderByDescending(x => x.Remark).ToList();
                            break;
                        case "UID":
                            queryTemp = queryTemp.OrderByDescending(x => x.Uid).ToList();
                            break;
                        default:
                            queryTemp = queryTemp.OrderByDescending(x => x.ConsentId).ToList();
                            break;
                    }
                }
                else
                {
                    //queryTemp = (List<GeneralConsent>)queryTemp.OrderBy(p => EF.Property<object>(p, sortColumn));
                    switch (sortColumn)
                    {
                        case "ConsentID":
                            queryTemp = queryTemp.OrderBy(x => x.ConsentId).ToList();
                            break;
                        case "CompanyID":
                            queryTemp = queryTemp.OrderBy(x => x.CompanyId).ToList();
                            break;
                        case "CollectionPointID":
                            queryTemp = queryTemp.OrderBy(x => x.CollectionPointId).ToList();
                            break;
                        case "ConsentDatetime":
                            queryTemp = queryTemp.OrderBy(x => x.ConsentDateTime).ToList();
                            break;
                        case "Email":
                            queryTemp = queryTemp.OrderBy(x => x.Email).ToList();
                            break;
                        case "NameSurname":
                            queryTemp = queryTemp.OrderBy(x => x.FullName).ToList();
                            break;
                        case "Tel":
                            queryTemp = queryTemp.OrderBy(x => x.PhoneNumber).ToList();
                            break;
                        case "FromBrowser":
                            queryTemp = queryTemp.OrderBy(x => x.FromBrowser).ToList();
                            break;
                        case "VerifyType":
                            queryTemp = queryTemp.OrderBy(x => x.VerifyType).ToList();
                            break;
                        case "CardNumber":
                            queryTemp = queryTemp.OrderBy(x => x.IdCardNumber).ToList();
                            break;
                        case "Remark":
                            queryTemp = queryTemp.OrderBy(x => x.Remark).ToList();
                            break;
                        case "UID":
                            queryTemp = queryTemp.OrderBy(x => x.Uid).ToList();
                            break;
                        case "Status":
                            queryTemp = queryTemp.OrderBy(x => x.Status).ToList();
                            break;
                        default:
                            queryTemp = queryTemp.OrderBy(x => x.ConsentId).ToList();
                            break;
                    }
                }

            }
            else
            {
                queryTemp = queryTemp.OrderBy(x => x.ConsentId).ToList();
            }


            #endregion

            #region Filter consent primary key
            if (request.GeneralConsentFilterKey != null)
            {
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.IdCardNumber))
                {
                    queryTemp = queryTemp.Where(p => p.IdCardNumber == request.GeneralConsentFilterKey.IdCardNumber).ToList();

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("idCardNumber", "No matching ID card number found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.FullName))
                {
                    queryTemp = queryTemp.Where(p => p.FullName == request.GeneralConsentFilterKey.FullName).ToList(); ;

                    if (query.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("fullName", "No matching full name found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.PhoneNumber))
                {
                    queryTemp = queryTemp.Where(p => p.PhoneNumber == request.GeneralConsentFilterKey.PhoneNumber).ToList(); ;

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("phoneNumber", "No matching phone number found"));

                        throw new ValidationException(failures);
                    }
                }
                if (!string.IsNullOrEmpty(request.GeneralConsentFilterKey.Email))
                {
                    queryTemp = queryTemp.Where(p => p.Email == request.GeneralConsentFilterKey.Email).ToList(); ;

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("email", "No matching email found"));

                        throw new ValidationException(failures);
                    }
                }
                if (request.GeneralConsentFilterKey.Uid != null)
                {
                    queryTemp = queryTemp.Where(p => p.Uid == request.GeneralConsentFilterKey.Uid).ToList(); ;

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("uid", "No matching UID found"));

                        throw new ValidationException(failures);
                    }
                }
                if (request.GeneralConsentFilterKey.StartDate != null)
                {
                    DateTimeOffset startDateVal = (DateTimeOffset)request.GeneralConsentFilterKey.StartDate;
                    queryTemp = queryTemp.Where(p => DateTimeOffset.Parse(p.ConsentDateTime) >= startDateVal).ToList();

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("startDate", "No matching start date found"));

                        throw new ValidationException(failures);
                    }
                }
                if (request.GeneralConsentFilterKey.EndDate != null)
                {
                    DateTimeOffset endDateVal = (DateTimeOffset)request.GeneralConsentFilterKey.EndDate;
                    queryTemp = queryTemp.Where(p => DateTimeOffset.Parse(p.ConsentDateTime) <= endDateVal).ToList();

                    if (queryTemp.Count() == 0)
                    {
                        List<ValidationFailure> failures = new List<ValidationFailure> { };
                        failures.Add(new ValidationFailure("endDate", "No matching end date found"));

                        throw new ValidationException(failures);
                    }
                }
            }
            #endregion
          
            foreach (var item in queryTemp)
            {
                item.CompanyId = (int)(long)item.CompanyId;
                item.CollectionPointId = (int)(long)item.CollectionPointId;
                item.Uid = (int)(long)item.Uid;
                item.TotalTransactions = (int)(long)item.TotalTransactions;
                item.FullName = (string)item.FullName;
                item.GeneralConsentWebsiteObject = websiteLookup[item.CollectionPointId.Value].FirstOrDefault();  //JSON
                item.CollectionPointGuid = (string)item.CollectionPointGuid;
                item.CollectionPointVersion = (int)(long)item.CollectionPointVersion;
                item.PurposeList = purposeLookup[item.CollectionPointId.Value].ToList(); // JSON 
                item.FromBrowser = (string)item.FromBrowser;
                item.PhoneNumber = (string)item.PhoneNumber;
                item.IdCardNumber = (string)item.IdCardNumber;
                item.Email = (string)item.Email;
                item.Remark = (string)item.Remark;
                //item.TotalCount = (int)item.TotalCount;
                item.CompanyId = (int)item.CompanyId;
                item.CompanyName = (string)item.CompanyName;
                item.Status = (string)item.Status;
                item.VerifyType = (string)item.VerifyType;
            }
            return queryTemp;

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
