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
    public Domain.Common.PaginationParams PaginationParams { get; set; }
    public Domain.Common.SortingParams SortingParams { get; set; }
    public Domain.Common.GeneralConsentFilterKey GeneralConsentFilterKey { get; set; }
    public string CurrentUtcOffset { get; init; }
    public string IsCurrentlyDst { get; init; }

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
            .ForMember(d => d.FullName, a => a.MapFrom(s => s.NameSurname))
            .ForMember(d => d.IdCardNumber, a => a.MapFrom(s => s.CardNumber))
            .ForMember(d => d.PhoneNumber, a => a.MapFrom(s => s.Tel))
            //.ForMember(d => d.ConsentDateTimeDisplay, a => a.MapFrom(s => s.ConsentDatetime.Value.LocalDateTime.ToShortDateString()))
            ;
        });

        Mapper mapper = new Mapper(config);
        //todo:edit conpanyid หลังมีการทำ identity server
        var query = _context.DbSetConsent.Where(consent => consent.CompanyId == 80158 && consent.New == 1 && consent.Status != "X");
        

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
                query = query.Where(p => p.NameSurname == request.GeneralConsentFilterKey.FullName);
            }
            if (request.GeneralConsentFilterKey.IdCardNumber != null)
            {
                query = query.Where(p => p.CardNumber == request.GeneralConsentFilterKey.IdCardNumber);
            }
            if (request.GeneralConsentFilterKey.PhoneNumber != null)
            {
                query = query.Where(p => p.Tel == request.GeneralConsentFilterKey.PhoneNumber);
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

        var pageNumber = 1;
        var pageSize = 10;

        if (request.PaginationParams != null)
        {
            pageNumber = request.PaginationParams.Offset.Value;
            pageSize = request.PaginationParams.Limit.Value;
        }
       

        PaginatedList<GeneralConsent> model = await query.ProjectTo<GeneralConsent>(mapper.ConfigurationProvider).PaginatedListAsync(pageNumber, pageSize);
       
        
        var collectionPointIds = model.Items.Select(c => c.CollectionPointId);

        #region get purpose
        var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
                           join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                           where cp.Status == Status.Active.ToString()
                           && p.Status == Status.Active.ToString()
                           && collectionPointIds.Contains(cp.CollectionPointId)
                           select new KeyValuePair<int, GeneralConsentPurpose>(cp.CollectionPointId,
                               new GeneralConsentPurpose
                               {
                                   PurposeId = p.PurposeId,
                                   //PurposeType = p.PurposeType == 1 ? PurposeType.Consent.ToString() : PurposeType.Cookie.ToString(),
                                   Code = p.Code,
                                   Description = p.Description,
                                   WarningDescription = p.WarningDescription,
                                   PurposeCategoryId = p.PurposeCategoryId,
                                   ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
                                   //Guid = p.Guid,
                                   Version = p.Version,
                                   Priority = cp.Priority,
                                   Status = p.Status,
                                   //CreateBy = p.CreateBy,
                                   //CreateDate = p.CreateDate.ToLocalTime(),
                                   //UpdateBy = p.UpdateBy,
                                   CompanyId = p.CompanyId,
                                   //UpdateDate = p.UpdateDate.ToLocalTime(),
                                   //Active = p.Status == "Active" ? true : false,
                               })).ToList();

        var purposeLookup = purposeList.ToLookup(item => item.Key, item => item.Value);
        #endregion

        #region get info collection point

     
        var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                 
                                   join c in _context.DbSetCompanies on cp.CompanyId equals (int)(long)c.CompanyId
                                   join w in _context.DbSetConsentWebsite on (int)(long)cp.CompanyId equals w.CompanyId
                                   where collectionPointIds.Contains(cp.CollectionPointId) && cp.CompanyId == 80158 && cp.Status != Status.X.ToString()
                                   select new KeyValuePair<int, CollectionPointInfo>(cp.CollectionPointId,
                                   new CollectionPointInfo
                                   {
                                       CollectionPointId = cp.CollectionPointId,
                                       //CollectionPoint = cp.CollectionPoint,
                                       //WebsiteId = cp.WebsiteId,
                                       //AccessToken = c.AccessToken,
                                       //Guid = cp.Guid,
                                       //WebsiteDescription = w.Description,
                                       //WebsiteUrl = w.Url,
                                       //WebsitePolicy = w.Urlpolicy,
                                       //Description = cp.Description,
                                       CompanyId = (int)(long)cp.CompanyId,
                                       Version = cp.Version,
                                       Status = cp.Status,
                                       //StatusDisplay = cp.Status == "Active" ? Status.Active.ToString() : Status.Inactive.ToString(),
                                       //CreateBy = cp.CreateBy,
                                       //CreateByDisplay = String.Format("{0} {1}", uCreate.FirstName, uCreate.LastName),
                                       //CreateDate = cp.CreateDate.ToLocalTime(),
                                       //CreateDateDisplay = c.CreateDate.LocalDateTime.ToShortDateString(),
                                       //UpdateBy = cp.UpdateBy,
                                       //UpdateByDisplay = String.Format("{0} {1}", uUpdate.FirstName, uUpdate.LastName),
                                       //IsStatus = cp.Status == "Active" ? true : false,
                                       //CollectionPointGuid = cp.CollectionPointId,
                                       
                                   })).ToList();


        #endregion
        string strPurpose = "";
        foreach (KeyValuePair<int, GeneralConsentPurpose> purpose in purposeList)
        {
            strPurpose = strPurpose + ","+purpose.Value.Description;

        }
        foreach (var item in model.Items)
        {
            item.PurposeList = purposeLookup[item.CollectionPointId.Value].ToList();
            //item.Purpose = strPurpose;
            //item.WebsiteDescription = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.WebsiteDescription).FirstOrDefault();
            item.CompanyId = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CompanyId).FirstOrDefault();
            item.Status = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Status).FirstOrDefault();
            //item.CreateBy = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateBy).FirstOrDefault();
            //item.CreateByDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateByDisplay).FirstOrDefault();
            //item.CreateDate = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateDate).FirstOrDefault();
            //item.CreateDateDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateDateDisplay).FirstOrDefault();
            //item.UpdateBy = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.UpdateBy).FirstOrDefault();
            //item.UpdateByDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.UpdateByDisplay).FirstOrDefault();
            //item.IsStatus = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.IsStatus).FirstOrDefault();
            //item.CollectionPointGuid = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Guid).FirstOrDefault();
            item.CollectionPointVersion = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Version).FirstOrDefault();
        }

        return model;
    }
}