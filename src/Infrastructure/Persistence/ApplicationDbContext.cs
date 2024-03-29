﻿using System.Reflection;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Infrastructure.Identity;
using WW.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WW.Application.CollectionPoints.Queries.GetCollectionPoints;
using Microsoft.Data.SqlClient;
using System.Data;
using WW.Application.GeneralConsents.Commands;
using AutoMapper;

namespace WW.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    public DbSet<Consent_CollectionPoint> DbSetConsentCollectionPoints => Set<Consent_CollectionPoint>();
    public DbSet<Consent_CollectionPointCustomField> DbSetConsentCollectionPointCustomFields => Set<Consent_CollectionPointCustomField>();
    public DbSet<Consent_SectionInfo> DbSetConsentSectionInfo => Set<Consent_SectionInfo>();
    public DbSet<Consent_Purpose> DbSetConsentPurpose => Set<Consent_Purpose>();
    public DbSet<Consent_PurposeCategory> DbSetConsentPurposeCategory => Set<Consent_PurposeCategory>();
    public DbSet<Consent_Page> DbSetConsentPage => Set<Consent_Page>();
    public DbSet<Consent_CollectionPointCustomFieldConfig> DbSetConsent_CollectionPointCustomFieldConfig => Set<Consent_CollectionPointCustomFieldConfig>();

    public DbSet<Consent_CollectionPointItem> DbSetConsentCollectionPointItem => Set<Consent_CollectionPointItem>();
    public DbSet<Companies> DbSetCompanies => Set<Companies>();
    public DbSet<ConsentWebsite> DbSetConsentWebsite => Set<ConsentWebsite>();
    public DbSet<Users> DbSetUser => Set<Users>();

    public DbSet<Consent_Consent> DbSetConsent => Set<Consent_Consent>();
    public DbSet<Consent_ConsentCustomField> DbSetConsentCustomField => Set<Consent_ConsentCustomField>();
    public DbSet<Consent_ConsentItem> DbSetConsentItem => Set<Consent_ConsentItem>();
    public DbSet<Consent_ConsentTheme> DbSetConsentTheme => Set<Consent_ConsentTheme>();
    public DbSet<Domain.Entities.File> DbSetFile => Set<Domain.Entities.File>();
    public DbSet<FileCategory> DbSetFileCategory => Set<FileCategory>();
    public DbSet<FileType> DbSetFileType => Set<FileType>();
    public DbSet<Position> DbSetPosition => Set<Position>(); 
    public DbSet<CompanyUser> DbSetCompanyUser => Set<CompanyUser>();
    public DbSet<LanguageDisplay> DbSetLanguage => Set<LanguageDisplay>();
    public DbSet<LocalStringResource> DbSetLocalStringResource => Set<LocalStringResource>();

    public DbSet<TotalRow> DbSetTotalRow => Set<TotalRow>();

    public DbSet<Domain.Entities.Action> DbSetAction => Set<Domain.Entities.Action>();
    public DbSet<Program> DbSetProgram => Set<Program>();
    public DbSet<ProgramAction> DbSetProgramAction => Set<ProgramAction>();
    public DbSet<ProgramDescription> DbSetProgramDescription => Set<ProgramDescription>();
    public DbSet<Role> DbSetRole => Set<Role>();
    public DbSet<RoleProgramAction> DbSetRoleProgramAction => Set<RoleProgramAction>();
    public DbSet<UserRole> DbSetUserRole => Set<UserRole>();

    public virtual DbSet<V_Consent_Latest_Consent> DbSetVConsentLatestConsents => Set<V_Consent_Latest_Consent>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<ScalarInt>().HasNoKey();

        builder.Entity<Consent_CollectionPoint>(entity =>
        {
            entity.HasKey(e => e.CollectionPointId)
                .HasName("PK__Consent___E8EFF6FE6D5E1394");

            entity.ToTable("Consent_CollectionPoint");

            entity.Property(e => e.CollectionPointId).HasColumnName("CollectionPointID");

            entity.Property(e => e.ActiveConsentCardNumberPk).HasColumnName("ActiveConsentCardNumberPK");

            entity.Property(e => e.ActiveConsentEmailPk).HasColumnName("ActiveConsentEmailPK");

            entity.Property(e => e.ActiveConsentNamePk).HasColumnName("ActiveConsentNamePK");

            entity.Property(e => e.ActiveConsentTelPk).HasColumnName("ActiveConsentTelPK");

            entity.Property(e => e.ActiveConsentUid).HasColumnName("ActiveConsentUID");

            entity.Property(e => e.ActiveConsentUidpk).HasColumnName("ActiveConsentUIDPK");

            entity.Property(e => e.ActiveConsentUidrequired).HasColumnName("ActiveConsentUIDRequired");

            entity.Property(e => e.CollectionPoint).HasMaxLength(1000);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Guid)
                .HasMaxLength(36)
                .HasColumnName("GUID");

            entity.Property(e => e.KeepAliveData)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.RedirectUrl).HasColumnType("ntext");

            entity.Property(e => e.Script).HasColumnType("ntext");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.WebsiteId).HasColumnName("WebsiteID");
        });

        builder.Entity<Consent_CollectionPointCustomField>(entity =>
        {
            entity.HasKey(e => e.CollectionPointCustomFieldId);

            entity.ToTable("Consent_CollectionPointCustomField");

            entity.Property(e => e.CollectionPointCustomFieldId).HasColumnName("CollectionPointCustomFieldID");

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.Owner).HasMaxLength(20);

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Type).HasMaxLength(100);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.Placeholder).HasMaxLength(100);

            entity.Property(e => e.LengthLimit).HasColumnName("LengthLimit");

            entity.Property(e => e.MaxLines).HasColumnName("MaxLines");

            entity.Property(e => e.MinLines).HasColumnName("MinLines");
        });

        builder.Entity<Consent_CollectionPointItem>(entity =>
        {
            entity.HasKey(e => e.CollectionPointItemId)
                .HasName("PK__Consent___466A10EACB14FFFE");

            entity.ToTable("Consent_CollectionPointItem");

            entity.Property(e => e.CollectionPointItemId).HasColumnName("CollectionPointItemID");

            entity.Property(e => e.CollectionPointId).HasColumnName("CollectionPointID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.PurposeId).HasColumnName("PurposeID");

            entity.Property(e => e.SectionInfoId).HasColumnName("SectionInfoID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<Consent_ConsentTheme>(entity =>
        {
            entity.HasKey(e => e.ThemeId);

            entity.ToTable("Consent_ConsentTheme");

            entity.Property(e => e.ThemeId).HasColumnName("ThemeId");

            entity.Property(e => e.ThemeTitle).HasMaxLength(100);

            entity.Property(e => e.HeaderTextColor).HasMaxLength(20);

            entity.Property(e => e.HeaderBackgroundColor).HasMaxLength(20);

            entity.Property(e => e.BodyBackgroundColor).HasMaxLength(20);

            entity.Property(e => e.TopDescriptionTextColor).HasMaxLength(20);

            entity.Property(e => e.BottomDescriptionTextColor).HasMaxLength(20);

            entity.Property(e => e.AcceptionButtonColor).HasMaxLength(20);

            entity.Property(e => e.AcceptionConsentTextColor).HasMaxLength(20);

            entity.Property(e => e.CancelButtonColor).HasMaxLength(20);

            entity.Property(e => e.CancelTextButtonColor).HasMaxLength(20);

            entity.Property(e => e.LinkToPolicyTextColor).HasMaxLength(20);

            entity.Property(e => e.PolicyUrlTextColor).HasMaxLength(20);

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

        });

        builder.Entity<Consent_SectionInfo>(entity =>
        {
            entity.HasKey(e => e.SectionInfoId)
                .HasName("PK__Consent___F3A82BFAE1699171");

            entity.ToTable("Consent_SectionInfo");

            entity.Property(e => e.SectionInfoId).HasColumnName("SectionInfoID");

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<Consent_Purpose>(entity =>
        {
            entity.HasKey(e => e.PurposeId)
                .HasName("PK__Purpose___79E6A1B493CC4A77");

            entity.ToTable("Consent_Purpose");

            entity.Property(e => e.PurposeId).HasColumnName("PurposeID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Guid)
                .HasMaxLength(36)
                .HasColumnName("GUID");

            entity.Property(e => e.PurposeType).HasColumnName("PurposeType");

            entity.Property(e => e.TextMoreDetail).HasColumnType("ntext");

            entity.Property(e => e.LinkMoreDetail).HasMaxLength(1000);

            entity.Property(e => e.PurposeCategoryId).HasColumnName("PurposeCategoryID");

            entity.Property(e => e.WarningDescription).HasColumnName("WarningDescription");

            entity.Property(e => e.KeepAliveData)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Language).HasMaxLength(2);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

        });


        builder.Entity<Consent_PurposeCategory>(entity =>
        {
            entity.HasKey(e => e.ID)
                .HasName("PK__Consent___3214EC274E856C2F");

            entity.ToTable("Consent_PurposeCategory");

            entity.Property(e => e.PurposeCategoryID).HasColumnName("PurposeCategoryID");

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Version).HasColumnName("Version");

            entity.Property(e => e.CreateBy).HasPrecision(0);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateBy).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Language).HasMaxLength(2);

        });


        builder.Entity<Consent_Page>(entity =>
        {
            entity.HasKey(e => e.PageId)
                .HasName("PK__Consent___C565B1240E4A4DD8");

            entity.ToTable("Consent_Page");

            entity.Property(e => e.PageId).HasColumnName("PageID");

            entity.Property(e => e.BodyBgcolor)
                .HasMaxLength(8)
                .HasColumnName("BodyBGColor");

            entity.Property(e => e.BodyBgimage).HasColumnName("BodyBGImage");

            entity.Property(e => e.BodyBottomDescription).HasColumnType("ntext");

            entity.Property(e => e.BodyBottomDescriptionFontColor).HasMaxLength(8);

            entity.Property(e => e.BodyTopDerscriptionFontColor).HasMaxLength(8);

            entity.Property(e => e.BodyTopDescription).HasColumnType("ntext");

            entity.Property(e => e.ButtonThankpage).HasMaxLength(100);

            entity.Property(e => e.CancelButtonBgcolor)
                .HasMaxLength(8)
                .HasColumnName("CancelButtonBGColor");

            entity.Property(e => e.CancelButtonFontColor).HasMaxLength(8);

            entity.Property(e => e.CollectionPointId).HasColumnName("CollectionPointID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.HeaderBgcolor)
                .HasMaxLength(8)
                .HasColumnName("HeaderBGColor");

            entity.Property(e => e.HeaderBgimage).HasColumnName("HeaderBGImage");

            entity.Property(e => e.HeaderFontColor).HasMaxLength(8);

            entity.Property(e => e.HeaderLabel).HasMaxLength(1000);

            entity.Property(e => e.HeaderLabelThankPage).HasMaxLength(1000);

            entity.Property(e => e.LabelActionCancel).HasMaxLength(100);

            entity.Property(e => e.LabelActionOk).HasMaxLength(100);

            entity.Property(e => e.LabelCheckBoxAccept).HasMaxLength(1000);

            entity.Property(e => e.LabelCheckBoxAcceptFontColor).HasMaxLength(8);

            entity.Property(e => e.LabelLinkToPolicy).HasMaxLength(1000);

            entity.Property(e => e.LabelLinkToPolicyFontColor).HasMaxLength(8);

            entity.Property(e => e.LabelLinkToPolicyUrl)
                .HasMaxLength(1000)
                .HasColumnName("LabelLinkToPolicyURL");

            entity.Property(e => e.LabelPurposeActionAgree).HasMaxLength(100);

            entity.Property(e => e.LabelPurposeActionNotAgree).HasMaxLength(100);

            entity.Property(e => e.LanguageCulture).HasMaxLength(10);

            entity.Property(e => e.OkbuttonBgcolor)
                .HasMaxLength(8)
                .HasColumnName("OKButtonBGColor");

            entity.Property(e => e.OkbuttonFontColor)
                .HasMaxLength(8)
                .HasColumnName("OKButtonFontColor");

            entity.Property(e => e.RedirectUrl).HasColumnType("ntext");

            entity.Property(e => e.ShortDescriptionThankPage).HasColumnType("ntext");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.UrlconsentPage)
                .HasMaxLength(1000)
                .HasColumnName("URLConsentPage");

            entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
        });
        builder.Entity<Consent_CollectionPointCustomFieldConfig>(entity =>
        {
            entity.HasKey(e => e.CollectionPointCustomFieldConfigId);

            entity.ToTable("Consent_CollectionPointCustomFieldConfig");

            entity.Property(e => e.CollectionPointCustomFieldConfigId).HasColumnName("CollectionPointCustomFieldConfigID");

            entity.Property(e => e.CollectionPointCustomFieldId).HasColumnName("CollectionPointCustomFieldID");

            entity.Property(e => e.CollectionPointGuid).HasMaxLength(36);
        });

        builder.Entity<Domain.Entities.File>(entity =>
        {
            entity.ToTable("File");

            entity.Property(e => e.FileId).HasColumnName("FileID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetimeoffset())");

            entity.Property(e => e.FileCategoryId).HasColumnName("FileCategoryID");

            entity.Property(e => e.FileTypeId).HasColumnName("FileTypeID");

            entity.Property(e => e.FullFileName).HasMaxLength(100);

            entity.Property(e => e.OriginalFileName).HasMaxLength(100);

            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('A')");

            entity.Property(e => e.ThumbFileName).HasMaxLength(100);

            entity.Property(e => e.UpdateDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetimeoffset())");
        });

        builder.Entity<FileCategory>(entity =>
        {
            entity.ToTable("FileCategory");

            entity.Property(e => e.FileCategoryId).HasColumnName("FileCategoryID");

            entity.Property(e => e.Code).HasMaxLength(100);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<FileType>(entity =>
        {
            entity.ToTable("FileType");

            entity.Property(e => e.FileTypeId).HasColumnName("FileTypeID");

            entity.Property(e => e.Code).HasMaxLength(100);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.PositionID).HasColumnName("PositionID");

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
        });

        builder.Entity<Companies>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.AccessToken)
                .HasMaxLength(1000)
                .HasColumnName("access_token");

            entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.CompanyGroupId).HasColumnName("CompanyGroupID");

            entity.Property(e => e.CompanyStatusId).HasColumnName("CompanyStatusID");

            entity.Property(e => e.CompanyTypeId).HasColumnName("CompanyTypeID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.DsrToken)
                .HasMaxLength(1000)
                .HasColumnName("DSR_token");

            entity.Property(e => e.ExpireDate).HasColumnName("expire_date");

            entity.Property(e => e.Fax).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("Name");

            entity.Property(e => e.Phone).HasMaxLength(100);

            entity.Property(e => e.ProfileImageId).HasColumnName("ProfileImageID");

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(1000)
                .HasColumnName("refresh_token");

            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('A')");

            entity.Property(e => e.TaxNo).HasMaxLength(20);

            entity.Property(e => e.TokenType)
                .HasMaxLength(1000)
                .HasColumnName("Token_type");

            entity.Property(e => e.TrialStatusId).HasColumnName("TrialStatusID");

            entity.Property(e => e.UpdateDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.LogoImage).HasColumnName("LogoImage");
        });

        builder.Entity<CompanyUser>(entity =>
        {
            entity.ToTable("CompanyUser");

            entity.Property(e => e.CompanyUserID).HasColumnName("CompanyUserID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UserID).HasColumnName("UserID");

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.DefaultBranch).HasColumnName("DefaultBranch");
        });

        builder.Entity<ConsentWebsite>(entity =>
        {
            entity.HasKey(e => e.WebsiteId)
                .HasName("PK__Consent___3F146A492FC12559");

            entity.ToTable("Consent_Website");

            entity.Property(e => e.WebsiteId).HasColumnName("WebsiteID");

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .HasColumnName("URL");

            entity.Property(e => e.Urlpolicy)
                .HasMaxLength(1000)
                .HasColumnName("URLPolicy");
        });

        builder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("PK__User___3F146A492FC12559");


            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.Property(e => e.Address).HasMaxLength(1000);

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.BirthDate).HasColumnType("date");

            entity.Property(e => e.ChangePwdate)
                .HasColumnType("date")
                .HasColumnName("ChangePWDate");

            entity.Property(e => e.CitizenId)
                .HasMaxLength(13)
                .HasColumnName("CitizenID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.DefaultLanguageId).HasColumnName("DefaultLanguageID");

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.FirstName).HasMaxLength(100);

            entity.Property(e => e.Gender).HasMaxLength(1);

            entity.Property(e => e.Guid)
                .HasMaxLength(36)
                .HasColumnName("GUID");

            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.Property(e => e.LoginFailEnd).HasPrecision(0);

            entity.Property(e => e.Password).HasMaxLength(1000);

            entity.Property(e => e.PositionId).HasColumnName("PositionID");

            entity.Property(e => e.ProfileImageId).HasColumnName("ProfileImageID");

            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("('A')");

            entity.Property(e => e.Tel).HasMaxLength(100);

            entity.Property(e => e.TitleId).HasColumnName("TitleID");

            entity.Property(e => e.UpdateDate)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetimeoffset())");

            entity.Property(e => e.Username).HasMaxLength(50);

            entity.Property(e => e.VerifyEmail).HasColumnName("verify_email");

            entity.Property(e => e.WorkStart).HasColumnType("date");
        });

        builder.Entity<Consent_Consent>(entity =>
        {
            entity.HasKey(e => e.ConsentId)
                .HasName("PK__Consent___374AB0A6B68CE3AD");

            entity.ToTable("Consent_Consent");

            entity.Property(e => e.ConsentId).HasColumnName("ConsentID");

            entity.Property(e => e.AgeRange).HasMaxLength(20);

            entity.Property(e => e.IdCardNumber).HasMaxLength(13).HasColumnName("CardNumber");

            entity.Property(e => e.CollectionPointId).HasColumnName("CollectionPointID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.ConsentDatetime).HasPrecision(0);

            entity.Property(e => e.ConsentSignature).HasColumnType("ntext");

            entity.Property(e => e.CreateDate).HasPrecision(0).HasColumnName("CreateDate");

            entity.Property(e => e.CreateBy).HasColumnName("Createby");

            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("Email");

            entity.Property(e => e.EventCode).HasMaxLength(1000).HasColumnName("EventCode");

            entity.Property(e => e.FromBrowser).HasMaxLength(1000).HasColumnName("FromBrowser");

            entity.Property(e => e.FromWebsite).HasMaxLength(1000).HasColumnName("FromWebsite");

            entity.Property(e => e.HasNotificationRenew).HasDefaultValueSql("((0))");

            entity.Property(e => e.FullName).HasMaxLength(1000).HasColumnName("NameSurname");

            entity.Property(e => e.Remark).HasMaxLength(1000).HasColumnName("Remark");

            entity.Property(e => e.Status).HasMaxLength(10).HasColumnName("Status");

            entity.Property(e => e.PhoneNumber).HasMaxLength(100).HasColumnName("Tel");

            entity.Property(e => e.Uid).HasColumnName("UID");

            entity.Property(e => e.UpdateDate).HasPrecision(0).HasColumnName("UpdateDate");

            entity.Property(e => e.VerifyType).HasMaxLength(100).HasColumnName("VerifyType");

            entity.Property(e => e.WebsiteId).HasColumnName("WebsiteID");
        });
        builder.Entity<TotalRow>(entity =>
        {
            entity.HasKey(e => e.TotalRowId)
                .HasName("PK__TotalRow__803CA0038B8A4C2D");


            entity.ToTable("TotalRow");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
            entity.Property(e => e.TableName).HasColumnName("TableName");
            entity.Property(e => e.TotalCountRow).HasColumnName("TotalCountRow");
            entity.Property(e => e.TotalCountGroup).HasColumnName("TotalCountGroup");
        });
        builder.Entity<Consent_ConsentCustomField>(entity =>
        {
            entity.HasKey(e => e.ConsentCustomFieldId)
                .HasName("PK_Consent_ConsentCustomField");

            entity.ToTable("Consent_ConsentCustomField");

            entity.Property(e => e.ConsentCustomFieldId).HasColumnName("Consent_ConsentCustomFieldID");
            entity.Property(e => e.CollectionPointCustomFieldConfigID).HasColumnName("CollectionPointCustomFieldConfigID");
            entity.Property(e => e.Value).HasColumnName("Value");
            entity.Property(e => e.ConsentId).HasColumnName("ConsentId");
     
        });
        builder.Entity<Domain.Entities.Action>(entity =>
        {
            entity.HasKey(e => e.ActionID).HasName("PK__Action__FFE3F4B936D0F612");

            entity.ToTable("Action");

            entity.Property(e => e.ActionID).HasColumnName("ActionID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgramID).HasName("PK__Program__7525603840AF8234");

            entity.ToTable("Program");

            entity.Property(e => e.ProgramID).HasColumnName("ProgramID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Code).HasMaxLength(50);

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.ParentID).HasColumnName("ParentID");

            entity.Property(e => e.Action).HasMaxLength(500);

            entity.Property(e => e.Icon).HasMaxLength(500);

            entity.Property(e => e.Badge).HasColumnName("Badge");

            entity.Property(e => e.expanded).HasColumnName("expanded");

            entity.Property(e => e.Priority).HasColumnName("Priority");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<ProgramAction>(entity =>
        {
            entity.HasKey(e => e.ProgramActionID).HasName("PK_ProgramAction");

            entity.ToTable("ProgramAction");

            entity.Property(e => e.ProgramActionID).HasColumnName("ProgramActionID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.ProgramID).HasColumnName("ProgramID");

            entity.Property(e => e.ActionID).HasColumnName("ActionID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<ProgramDescription>(entity =>
        {
            entity.HasKey(e => e.ProgramDescriptionID).HasName("PK_ProgramDescription");

            entity.ToTable("ProgramDescription");

            entity.Property(e => e.ProgramDescriptionID).HasColumnName("ProgramDescriptionID");

            entity.Property(e => e.ProgramID).HasColumnName("ProgramID");

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.LanguageCulture).HasMaxLength(10);
        });

        builder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A7816E557");

            entity.ToTable("Role");

            entity.Property(e => e.RoleID).HasColumnName("RoleID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        builder.Entity<RoleProgramAction>(entity =>
        {
            entity.HasKey(e => e.RoleProgramActionID).HasName("PK__RoleProg__46F87EF81A713C42");

            entity.ToTable("RoleProgramAction");

            entity.Property(e => e.RoleProgramActionID).HasColumnName("RoleProgramActionID");

            entity.Property(e => e.RoleID).HasColumnName("RoleID");

            entity.Property(e => e.ProgramActionID).HasColumnName("ProgramActionID");
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleID).HasName("PK__UserRole__3D978A5565ED7DB2");

            entity.ToTable("UserRole");

            entity.Property(e => e.UserRoleID).HasColumnName("UserRoleID");

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.RoleID).HasColumnName("RoleID");

            entity.Property(e => e.UserID).HasColumnName("UserID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);
        });

        /*modelBuilder.Entity<ConsentConsentCookie>(entity =>
        {
            entity.HasKey(e => e.ConsentCookieId)
                .HasName("PK__Consent___4376938F0561F3DB");

            entity.ToTable("Consent_ConsentCookie");

            entity.Property(e => e.ConsentCookieId).HasColumnName("ConsentCookieID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Message).HasColumnType("ntext");

            entity.Property(e => e.PopupMessage).HasColumnType("ntext");

            entity.Property(e => e.Script).HasColumnType("ntext");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.WebsiteId).HasColumnName("WebsiteID");
        });

        modelBuilder.Entity<ConsentConsentCustomField>(entity =>
        {
            entity.ToTable("Consent_ConsentCustomField");

            entity.Property(e => e.ConsentConsentCustomFieldId).HasColumnName("Consent_ConsentCustomFieldID");

            entity.Property(e => e.CollectionPointCustomFieldConfigId).HasColumnName("CollectionPointCustomFieldConfigID");

            entity.Property(e => e.ConsentId).HasColumnName("ConsentID");

            entity.Property(e => e.Value).HasMaxLength(1000);
        });
       */
        builder.Entity<Consent_ConsentItem>(entity =>
        {
            entity.HasKey(e => e.ConsentItemId)
                .HasName("PK__Consent___2CDB7A9952057809");

            entity.ToTable("Consent_ConsentItem");

            entity.Property(e => e.ConsentItemId).HasColumnName("ConsentItemID");

            entity.Property(e => e.CollectionPointItemId).HasColumnName("CollectionPointItemID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.ConsentId).HasColumnName("ConsentID");
        });
        builder.Entity<V_Consent_Latest_Consent>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("V_CONSENT_LATEST_CONSENT");

            entity.Property(e => e.AgeRange).HasMaxLength(20);

            entity.Property(e => e.CardNumber).HasMaxLength(13);

            entity.Property(e => e.CollectionPointId).HasColumnName("CollectionPointID");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

            entity.Property(e => e.ConsentDatetime).HasPrecision(0);

            entity.Property(e => e.ConsentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ConsentID");

            entity.Property(e => e.ConsentSignature).HasColumnType("ntext");

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.EventCode).HasMaxLength(1000);

            entity.Property(e => e.FromBrowser).HasMaxLength(1000);

            entity.Property(e => e.FromWebsite).HasMaxLength(1000);

            entity.Property(e => e.NameSurname).HasMaxLength(1000);

            entity.Property(e => e.Remark).HasMaxLength(1000);

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.Tel).HasMaxLength(100);

            entity.Property(e => e.Uid).HasColumnName("UID");

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.VerifyType).HasMaxLength(100);

            entity.Property(e => e.WebsiteId).HasColumnName("WebsiteID");
        });

        builder.Entity<LanguageDisplay>(entity =>
        {
            entity.HasKey(e => e.LanguageID)
                .HasName("PK__Language___2CDB7A9952057809");

            entity.ToTable("Language");

            entity.Property(e => e.Version).HasColumnName("Version");

            entity.Property(e => e.Status).HasMaxLength(10);

            entity.Property(e => e.CreateBy).HasPrecision(0);

            entity.Property(e => e.CreateDate).HasPrecision(0);

            entity.Property(e => e.UpdateBy).HasPrecision(0);

            entity.Property(e => e.UpdateDate).HasPrecision(0);

            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Name).HasColumnName("Name");

            entity.Property(e => e.LanguageCulture).HasColumnName("LanguageCulture");

            entity.Property(e => e.IconUrl).HasColumnName("IconUrl");

            entity.Property(e => e.CompanyID).HasColumnName("CompanyID");

            entity.Property(e => e.DisplayOrder).HasColumnName("DisplayOrder");

            entity.Property(e => e.IsDefault).HasColumnName("IsDefault");
        });

        builder.Entity<LocalStringResource>(entity =>
        {
            entity.HasKey(e => e.LocalStringResourceID)
                .HasName("PK__LocalStringResource___2CDB7A9952057809");

            entity.ToTable("LocalStringResource");

            entity.Property(e => e.ResourceKey).HasColumnName("ResourceKey");

            entity.Property(e => e.ResourceValue).HasColumnName("ResourceValue");

            entity.Property(e => e.LanguageCulture).HasColumnName("LanguageCulture");

            
        });

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
    /*public async Task<int> SubmitConsent(string query, params SqlParameter[] parameters)
    {
    }*/

    public void OpenConnection()
    {
        Database.OpenConnection();
    }
    public void CloseConnection()
    {
        Database.CloseConnection();
    }

    //public async Task<int> SubmitConsent(string query
    //                    /*, int CompanyId
    //                    , string CollectionPointGuid
    //                    , int WebSiteId
    //                    , string FullName
    //                    , string Email
    //                    , string PhoneNumber
    //                    , string FromBrowser
    //                    , string FromWebsite
    //                    , string VerifyType
    //                    , string ConsentSignature
    //                    , string IdCardNumber
    //                    , int createBy
    //                    , DateTimeOffset ExpiredDateTime
    //                    , string EventCode
    //                    , int Uid
    //                    , string AgeRangeCode*/
    //    , SubmitConsentCommand request)
    //{

    //    int id = 0;
    //    try
    //    {
    //        #region set sqlParameter

    //        var outputParam = new SqlParameter
    //        {
    //            ParameterName = "@OutputID",
    //            SqlDbType = SqlDbType.Int,
    //            Direction = ParameterDirection.Output
    //        };

    //        var companyParam = new SqlParameter
    //        {
    //            ParameterName = "@CompanyID",
    //            Value = request.CompanyId,
    //            SqlDbType = SqlDbType.Int,

    //        };

    //        var collectionPointGUIDParam = new SqlParameter
    //        {
    //            ParameterName = "@CollectionPointGUID",
    //            Value = request.CollectionPointGuid,
    //            SqlDbType = SqlDbType.NVarChar

    //        };

    //        var websiteIDParam = new SqlParameter
    //        {
    //            ParameterName = "@WebsiteID",
    //            Value = request.WebSiteId,
    //            SqlDbType = SqlDbType.Int
    //        };

    //        var nameSurnameParam = new SqlParameter
    //        {
    //            ParameterName = "@NameSurname",
    //            Value = request.FullName,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var emailParam = new SqlParameter
    //        {
    //            ParameterName = "@Email",
    //            Value = request.Email,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var telParam = new SqlParameter
    //        {
    //            ParameterName = "@Tel",
    //            Value = request.PhoneNumber,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var fromBrowserParam = new SqlParameter
    //        {
    //            ParameterName = "@FromBrowser",
    //            Value = request.FromBrowser,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var fromWebsiteParam = new SqlParameter
    //        {
    //            ParameterName = "@FromWebsite",
    //            Value = request.FromWebsite,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var verifyTypeParam = new SqlParameter
    //        {
    //            ParameterName = "@VerifyType",
    //            Value = request.VerifyType,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var consentSignatureParam = new SqlParameter
    //        {
    //            ParameterName = "@ConsentSignature",
    //            Value = request.ConsentSignature,
    //            SqlDbType = SqlDbType.NText
    //        };

    //        var cardNumberParam = new SqlParameter
    //        {
    //            ParameterName = "@CardNumber",
    //            Value = request.IdCardNumber,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var createByParam = new SqlParameter
    //        {
    //            ParameterName = "@CreateBy",
    //            SqlDbType = SqlDbType.Int,
    //            Value = 1
    //        };

    //        var expiredParam = new SqlParameter
    //        {
    //            ParameterName = "@Expired",
    //            Value = request.ExpiredDateTime,
    //            SqlDbType = SqlDbType.DateTimeOffset

    //        };

    //        var eventCodeParam = new SqlParameter
    //        {
    //            ParameterName = "@EventCode",
    //            Value = request.EventCode,
    //            SqlDbType = SqlDbType.NVarChar
    //        };

    //        var uIDParam = new SqlParameter
    //        {
    //            ParameterName = "@UID",
    //            Value = request.Uid,
    //            SqlDbType = SqlDbType.Int
    //        };

    //        var ageRangeParam = new SqlParameter
    //        {
    //            ParameterName = "@AgeRange",
    //            Value = request.AgeRangeCode,
    //            SqlDbType = SqlDbType.NVarChar
    //        };
    //        #endregion

    //        var result = await Database.ExecuteSqlRawAsync("EXEC" + query
    //                        , companyParam
    //                        , collectionPointGUIDParam
    //                        , websiteIDParam
    //                        , nameSurnameParam
    //                        , emailParam
    //                        , telParam
    //                        , fromBrowserParam
    //                        , fromWebsiteParam
    //                        , verifyTypeParam
    //                        , consentSignatureParam
    //                        , cardNumberParam
    //                        , createByParam
    //                        , expiredParam
    //                        , eventCodeParam
    //                        , uIDParam
    //                        , ageRangeParam
    //                        , outputParam).ConfigureAwait(false);


    //        id = Convert.ToInt32(outputParam.Value.ToString());
    //        var consentID = new SqlParameter
    //        {
    //            ParameterName = "@ConsentID",
    //            Value = id,
    //            SqlDbType = SqlDbType.Int
    //        };

    //        foreach (var purpose in request.Purpose)
    //        {


    //            var purposeGUID = new SqlParameter
    //            {
    //                ParameterName = "@PurposeGUID",
    //                Value = purpose.PurposeGuid,
    //                SqlDbType = SqlDbType.UniqueIdentifier
    //            };
    //            var consentActive = new SqlParameter
    //            {
    //                ParameterName = "@ConsentActive",
    //                Value = purpose.Active,
    //                SqlDbType = SqlDbType.Bit

    //            };
    //            var expired = new SqlParameter
    //            {
    //                ParameterName = "@Expired",
    //                //Value = purpose.ExpiredDateTime,
    //                SqlDbType = SqlDbType.DateTimeOffset
    //            };
                

    //            var result2 = Database.ExecuteSqlRaw("EXEC" + "[SP_CONSENT_SIGNITEM] @CompanyID,@ConsentID,@CollectionPointGUID,@PurposeGUID,@ConsentActive,@Expired", companyParam, consentID, collectionPointGUIDParam, purposeGUID, consentActive, expired);
    //        }

    //        foreach (var collectionPointCustomField in request.CollectionPointCustomField)
    //        {
    //            var collectionPointCustomFieldConfigID = new SqlParameter
    //            {
    //                ParameterName = "@CollectionPointCustomFieldConfigID",
    //                Value = collectionPointCustomField.CollectionPointCustomFieldConfigId,
    //                SqlDbType = SqlDbType.Int

    //            };
    //            var valueParam = new SqlParameter
    //            {
    //                ParameterName = "@Value",
    //                Value = collectionPointCustomField.Value,
    //                SqlDbType = SqlDbType.NVarChar
    //            };
    //            var result3 = Database.ExecuteSqlRaw("EXEC" + "[SP_CONSENT_CONSENT_CUSTOM_FIELD] @CollectionPointCustomFieldConfigID, @Value,@ConsentID", collectionPointCustomFieldConfigID, valueParam, consentID);
    //        }

    //        return id;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(query, ex);
    //    }

    //}
}
