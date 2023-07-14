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
using WW.Domain.Enums;

namespace WW.Application.ConsentPageSetting.Queries.GetAllShortURL;
public record GetAllShortURLQuery : IRequest<List<ShortUrl>>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class GetAllShortURLHandler : IRequestHandler<GetAllShortURLQuery, List<ShortUrl>>
{
    private readonly IApplicationDbContext _context;
    private readonly IGenerateURLService _generateURLService;

    public GetAllShortURLHandler(IApplicationDbContext context, IGenerateURLService generateURLService)
    {
        _context = context;
        _generateURLService = generateURLService;
    }

    public async Task<List<ShortUrl>> Handle(GetAllShortURLQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }

        try
        {
            var completelyConsentForm = (from cs in _context.DbSetConsentCollectionPoints
                                         join c in _context.DbSetCompanies on cs.CompanyId equals Convert.ToInt32(c.CompanyId)
                                         join cp in _context.DbSetConsentPage on cs.CollectionPointId equals cp.CollectionPointId
                                         join ct in _context.DbSetConsentTheme on cp.ThemeId equals ct.ThemeId
                                         where Convert.ToInt32(c.CompanyId) == request.authentication.CompanyID && c.Status == ('A').ToString() &&
                                            cs.CompanyId == request.authentication.CompanyID && cs.Status == Status.Active.ToString() &&
                                            cp.CompanyId == request.authentication.CompanyID && cp.Status == Status.Active.ToString() &&
                                            ct.CompanyId == request.authentication.CompanyID && ct.Status == Status.Active.ToString()
                                         select new
                                         {
                                             ConsentId = cs.CollectionPointId,
                                             ConsentTitle = cs.CollectionPoint,
                                             CompanyId = c.CompanyId,
                                             ConsentPageId = cp.PageId,
                                             ConsentThemeId = ct.ThemeId,
                                         }).ToList();

            var shortUrls = new List<ShortUrl> { };

            if (completelyConsentForm.Count != 0)
            {
                foreach (var consent in completelyConsentForm)
                {
                    GenerateURLModel model = await _generateURLService.GenerateShortenURL(
                        new GenerateURLModel{
                            long_url = "https://test-pdpa.thewiseworks.com/CMSConsent/ConsentPage?code=9TyqBi9YFUfna9JreXsoKrcp5M9YT2jcTuJ3WL%2B%2Bw%2FdQPb7Vp%2FAVmeHfW0UYb%2B4Ermnkeu57M5Hzxuq%2BrmdqI96nfQ9g2ArMAhUHe8UlXqJ8yJ3qB4fbYifojxMHt4uFbjLHCFbNNmTSeq2u3d8iL%2FJZbGrEUVN8eysx1bBDsh2JDx2lVbdJUTKFkXLHIEwJzb833gP4aAtPs3ZjAcTF5DHI8nuZRNPGL5s5kwtrLRXrxx9se0qjSpkoKpbfV934SKj4PVJvhol28xxkoFEQLmUZbHwF1GdWP9kSN2kl3mAyrBnuM4fnGm8%2FUgzoosNzp4Qocwqi%2F2lQv26tJQhI5U7u3cgmuOQ0pcU80%2FRKP3VIIJ1cAwSNw0VfA48X6jE7",
                            id = consent.ConsentId.ToString(),
                            link = ""
                        });

                    ShortUrl shortUrl = new ShortUrl { 
                        Url = model.link
                    };

                    shortUrls.Add(shortUrl);
                }
            }

            return shortUrls;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    }
}
