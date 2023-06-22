using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using NSwag.Annotations;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.ConsentPageSetting.Commands.UpdateConsentTheme;

public record UpdateConsentThemeCommand : IRequest<ConsentTheme>
{
    [JsonIgnore]
    public int themeId { get; set; }
    public string themeTitle { get; init; }
    public string headerTextColor { get; init; }
    public string headerBackgroundColor { get; init; }
    public string bodyBackgroudColor { get; init; }
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

public class UpdateConsentThemeCommandHandler : IRequestHandler<UpdateConsentThemeCommand, ConsentTheme>
{
    private readonly IApplicationDbContext _context;

    public UpdateConsentThemeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConsentTheme> Handle(UpdateConsentThemeCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (request.themeId <= 0)
        {
            List<ValidationFailure> failures = new List<ValidationFailure> { };
            failures.Add(new ValidationFailure("id", "ID must be greater than 0"));

            throw new ValidationException(failures);
        }

        var entity = _context.DbSetConsentTheme
            .Where(cf => cf.ThemeId == request.themeId && cf.CompanyId == request.authentication.CompanyID && cf.Status != Status.X.ToString())
            .FirstOrDefault();

        if (entity == null)
        {
            throw new NotFoundException();
        }

        try
        {
            entity.ThemeTitle = request.themeTitle;
            entity.HeaderTextColor = request.headerTextColor;
            entity.HeaderBackgroundColor = request.headerBackgroundColor;
            entity.BodyBackgroudColor = request.bodyBackgroudColor;
            entity.TopDescriptionTextColor = request.topDescriptionTextColor;
            entity.BottomDescriptionTextColor = request.bottomDescriptionTextColor;
            entity.AcceptionButtonColor = request.acceptionButtonColor;
            entity.AcceptionConsentTextColor = request.acceptionConsentTextColor;
            entity.CancelButtonColor = request.cancelButtonColor;
            entity.CancelTextButtonColor = request.cancelTextButtonColor;
            entity.LinkToPolicyTextColor = request.linkToPolicyTextColor;
            entity.PolicyUrlTextColor = request.policyUrlTextColor;

            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.Version += 1;

            _context.DbSetConsentTheme.Update(entity);

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
