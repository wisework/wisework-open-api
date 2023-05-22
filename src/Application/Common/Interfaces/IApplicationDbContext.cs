﻿using WW.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using System.Data.SqlClient;
using WW.Application.GeneralConsents.Commands;

namespace WW.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Consent_CollectionPoint> DbSetConsentCollectionPoints { get; }
    DbSet<Consent_SectionInfo> DbSetConsentSectionInfo { get; }
    DbSet<Consent_Purpose> DbSetConsentPurpose { get; }
    DbSet<Consent_Page> DbSetConsentPage { get; }
    DbSet<Consent_CollectionPointItem> DbSetConsentCollectionPointItem { get; }
    DbSet<Consent_CollectionPointCustomField> DbSetConsentCollectionPointCustomFields { get; }
    DbSet<Consent_CollectionPointCustomFieldConfig> DbSetConsent_CollectionPointCustomFieldConfig { get; }

    DbSet<Companies> DbSetCompanies { get; }
    DbSet<ConsentWebsite> DbSetConsentWebsite { get; } 
    DbSet<Users> DbSetUser { get; }
    DbSet<Consent_Consent> DbSetConsent { get; }
    DbSet<V_Consent_Latest_Consent> DbSetVConsentLatestConsents {get; }
    DbSet<Consent_ConsentItem> DbSetConsentItem { get; }
    DbSet<Consent_ConsentTheme> DbSetConsentTheme { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SubmitConsent(string query
                        /*,int CompanyId
                        , string CollectionPointGuid
                        , int WebSiteId
                        , string FullName
                        , string Email
                        , string PhoneNumber
                        , string FromBrowser
                        , string FromWebsite
                        , string VerifyType
                        , string ConsentSignature
                        , string IdCardNumber
                        , int createBy
                        , DateTimeOffset ExpiredDateTime
                        , string EventCode
                        , int Uid
                        , string AgeRangeCode*/
        ,SubmitConsentCommand command);
    void OpenConnection();
    void CloseConnection();
}
