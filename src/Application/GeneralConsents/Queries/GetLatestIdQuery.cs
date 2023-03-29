

using AutoMapper;
using MediatR;
using WW.Application.Common.Interfaces;
using WW.Domain.Enums;

namespace WW.Application.GeneralConsents.Queries;
public record GetLatestIdRequestQuery : IRequest<int>
{
    public string IdCardNumber { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CollectionPointGuid { get; set; }

}


public class GetLatestIdRequestQueryHandler : IRequestHandler<GetLatestIdRequestQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLatestIdRequestQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<int> Handle(GetLatestIdRequestQuery request, CancellationToken cancellationToken)
    {
        var query = (from c in _context.DbSetConsent
                     join cp in _context.DbSetConsentCollectionPoints on c.CollectionPointId equals cp.CollectionPointId
                     where cp.Guid == request.CollectionPointGuid && c.CompanyId == 80158
                     && c.NameSurname == request.FullName
                     || c.Email == request.Email
                     || c.Tel == request.PhoneNumber
                     || c.CardNumber == request.IdCardNumber
                     select c.ConsentId).ToList().FirstOrDefault();


        return Task.FromResult(query);
    }
}

