using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.ConsentPageSetting.Commands.CreateConsentTheme;

public record CreateConsentThemeCommand : IRequest<ConsentTheme>
{
    public string themeTitle { get; init; }
    public string headerTextColor { get; init; }
    public string headerBackgroundColor { get; init; }
    public string bodyBackgroundColor { get; init; }
    public string topDescriptionTextColor { get; init; }
    public string bottomDescriptionTextColor { get; init; }
    public string acceptionButtonColor { get; init; }
    public string acceptionConsentTextColor { get; init; }
    public string cancelButtonColor { get; init; }
    public string cancelTextButtonColor { get; init; }
    public string linkToPolicyTextColor { get; init; }
    public string policyUrlTextColor { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class CreateConsentThemeCommandHandler : IRequestHandler<CreateConsentThemeCommand, ConsentTheme>
{
    private readonly IApplicationDbContext _context;

    public CreateConsentThemeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConsentTheme> Handle(CreateConsentThemeCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        try
        {
            var entity = new Consent_ConsentTheme();

            entity.ThemeTitle = request.themeTitle;
            entity.HeaderTextColor = request.headerTextColor;
            entity.HeaderBackgroundColor = request.headerBackgroundColor;
            entity.BodyBackgroudColor = request.bodyBackgroundColor;
            entity.TopDescriptionTextColor = request.topDescriptionTextColor;
            entity.BottomDescriptionTextColor = request.bottomDescriptionTextColor;
            entity.AcceptionButtonColor = request.acceptionButtonColor;
            entity.AcceptionConsentTextColor = request.acceptionConsentTextColor;
            entity.CancelButtonColor = request.cancelButtonColor;
            entity.CancelTextButtonColor = request.cancelTextButtonColor;
            entity.LinkToPolicyTextColor = request.linkToPolicyTextColor;
            entity.PolicyUrlTextColor = request.policyUrlTextColor;

            entity.CreateBy = request.authentication.UserID;
            entity.UpdateBy = request.authentication.UserID;
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version = 1;
            entity.CompanyId = request.authentication.CompanyID;

            _context.DbSetConsentTheme.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var consentThemeInfo = new ConsentTheme
            {
                ThemeId = entity.ThemeId,
                ThemeTitle = entity.ThemeTitle,
                HerderTextColor = entity.HeaderTextColor,
                HeaderBackgroundColor = entity.HeaderBackgroundColor,
                BodyBackgroudColor = entity.BodyBackgroudColor,
                TopDescriptionTextColor = entity.TopDescriptionTextColor,
                BottomDescriptionTextColor = entity.BottomDescriptionTextColor,
                AcceptionButtonColor = entity.AcceptionButtonColor,
                AcceptionConsentTextColor = entity.AcceptionConsentTextColor,
                CancelButtonColor = entity.CancelButtonColor,
                CancelTextButtonColor = entity.CancelTextButtonColor,
                LinkToPolicyTextColor = entity.LinkToPolicyTextColor,
                PolicyUrlTextColor = entity.PolicyUrlTextColor,
                Status = entity.Status,
                CompanyId = entity.CompanyId,
            };

            return consentThemeInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
