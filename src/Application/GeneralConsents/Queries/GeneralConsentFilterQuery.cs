using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;
namespace WW.Application.GeneralConsents.Queries;

public record GeneralConsentListRequestQuery : IRequest<PaginatedList<GeneralConsent>>
{
    public Domain.Common.PaginationParams? PaginationParams { get; set; }
    public Domain.Common.SortingParams SortingParams { get; set; }
    public Domain.Common.GeneralConsentFilterKey GeneralConsentFilterKey { get; set; }

}

public class GeneralConsentListRequestQueryHandler : IRequestHandler<GeneralConsentListRequestQuery, PaginatedList<GeneralConsent>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GeneralConsentListRequestQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GeneralConsent>> Handle(GeneralConsentListRequestQuery request, CancellationToken cancellationToken)
    {
       
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_Consent, GeneralConsent>()
            .ForMember(d => d.ConsentId, a => a.MapFrom(s => s.ConsentId))
            .ForMember(d => d.FullName, a => a.MapFrom(s => s.FullName))
            .ForMember(d => d.IdCardNumber, a => a.MapFrom(s => s.IdCardNumber))
            .ForMember(d => d.PhoneNumber, a => a.MapFrom(s => s.PhoneNumber))
            //.ForMember(d => d.ConsentDateTimeDisplay, a => a.MapFrom(s => s.ConsentDatetime.Value.LocalDateTime.ToShortDateString()))
            ;
        });

        Mapper mapper = new Mapper(config);
        //todo:edit conpanyid หลังมีการทำ identity server
        var query = _context.DbSetConsent.Where(consent => consent.CompanyId == 1 && consent.New == 1 && consent.Status != "X");
        

        #region Sorting 
       if(request.SortingParams != null)
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
        if (request.GeneralConsentFilterKey != null) {
            if (request.GeneralConsentFilterKey.FullName != null)
            {
                query = query.Where(p => p.FullName == request.GeneralConsentFilterKey.FullName);
            }
            if (request.GeneralConsentFilterKey.IdCardNumber != null)
            {
                query = query.Where(p => p.IdCardNumber == request.GeneralConsentFilterKey.IdCardNumber);
            }
            if (request.GeneralConsentFilterKey.PhoneNumber != null)
            {
                query = query.Where(p => p.PhoneNumber == request.GeneralConsentFilterKey.PhoneNumber);
            }
            if (request.GeneralConsentFilterKey.Email != null)
            {
                query = query.Where(p => p.Email == request.GeneralConsentFilterKey.Email);
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

        if (request.PaginationParams != null)
        {
            Offset = request.PaginationParams.Offset.Value;
            Limit = request.PaginationParams.Limit.Value;
        }
       

        PaginatedList<GeneralConsent> model = await query
            .ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(Offset, Limit);

        //PaginatedList<GeneralConsent> model =
        //   await _context.DbSetConsent.Where(p => p.CompanyId == 1 && p.Status == Status.Active.ToString())
        //   .ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(request.Offset, request.Limit);

        //var collectionPointIds = model.Items.Select(c => c.CollectionPointId);

        //#region get purpose
        //var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
        //                   join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
        //                   where cp.Status == Status.Active.ToString()
        //                   && p.Status == Status.Active.ToString()
        //                   && collectionPointIds.Contains(cp.CollectionPointId)
        //                   select new KeyValuePair<int, GeneralConsentPurpose>(cp.CollectionPointId,
        //                       new GeneralConsentPurpose
        //                       {
        //                           PurposeId = p.PurposeId,
        //                           //PurposeType = p.PurposeType == 1 ? PurposeType.Consent.ToString() : PurposeType.Cookie.ToString(),
        //                           Code = p.Code,
        //                           Description = p.Description,
        //                           WarningDescription = p.WarningDescription,
        //                           PurposeCategoryId = p.PurposeCategoryId,
        //                           ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
        //                           //Guid = p.Guid,
        //                           Version = p.Version,
        //                           Priority = cp.Priority,
        //                           Status = p.Status,
        //                           /*CreateBy = p.CreateBy,
        //                           CreateDate = p.CreateDate.ToLocalTime(),
        //                           UpdateBy = p.UpdateBy,*/
        //                           CompanyId = p.CompanyId,
        //                           /*UpdateDate = p.UpdateDate.ToLocalTime(),
        //                           Active = p.Status == "Active" ? true : false,*/
        //                       })).ToList();

        //var purposeLookup = purposeList.ToLookup(item => item.Key, item => item.Value);
        //#endregion

        //#region get info collection point 
        //var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                 
        //                           join c in _context.DbSetCompanies on cp.CompanyId equals (int)(long)c.CompanyId
        //                           join w in _context.DbSetConsentWebsite on (int)(long)cp.CompanyId equals w.CompanyId
        //                           where collectionPointIds.Contains(cp.CollectionPointId) && cp.CompanyId == 80158 && cp.Status != Status.X.ToString()
        //                           select new KeyValuePair<int, CollectionPointInfo>(cp.CollectionPointId,
        //                           new CollectionPointInfo
        //                           {
        //                               CollectionPointId = cp.CollectionPointId,
        //                               /*CollectionPoint = cp.CollectionPoint,
        //                               WebsiteId = cp.WebsiteId,
        //                               AccessToken = c.AccessToken,
        //                               Guid = cp.Guid,
        //                               WebsiteDescription = w.Description,
        //                               WebsiteUrl = w.Url,
        //                               WebsitePolicy = w.Urlpolicy,
        //                               Description = cp.Description,*/
        //                               CompanyId = (int)(long)cp.CompanyId,
        //                               Version = cp.Version,
        //                               Status = cp.Status,
        //                               /*StatusDisplay = cp.Status == "Active" ? Status.Active.ToString() : Status.Inactive.ToString(),
        //                               CreateBy = cp.CreateBy,*/
        //                               //CreateByDisplay = String.Format("{0} {1}", uCreate.FirstName, uCreate.LastName),
        //                               /*CreateDate = cp.CreateDate.ToLocalTime(),
        //                               CreateDateDisplay = c.CreateDate.LocalDateTime.ToShortDateString(),
        //                               UpdateBy = cp.UpdateBy,*/
        //                               //UpdateByDisplay = String.Format("{0} {1}", uUpdate.FirstName, uUpdate.LastName),
        //                               //IsStatus = cp.Status == "Active" ? true : false,
        //                               //CollectionPointGuid = cp.CollectionPointId,
                                       
        //                           })).ToList();
        //#endregion

        var collectionpoints = _context.DbSetConsentCollectionPoints.Where(cp => cp.CompanyId == 1 && cp.Status != Status.X.ToString()).ToList();
        var companies = _context.DbSetCompanies.Where(c => c.Status == "A").ToList();
        var webSites = _context.DbSetConsentWebsite.Where(ws => ws.CompanyId == 1 && ws.Status != Status.X.ToString()).ToList();
        var purposes = _context.DbSetConsentPurpose.Where(p => p.CompanyId == 1 && p.Status != Status.X.ToString()).ToList();

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
                           select new KeyValuePair<int, Website4>(cp.CollectionPointId,
                               new Website4
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
            item.Website = websiteLookup[item.CollectionPointId.Value].FirstOrDefault();  //JSON
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
}