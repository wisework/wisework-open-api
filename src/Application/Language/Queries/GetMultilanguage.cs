using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Enums;

namespace WW.Application.Language.Queries;

public record GetMultilanguage(string LanguageCulture, string key) : IRequest<LanguageCulture>;


public class GetMultilanguageQueryHandler : IRequestHandler<GetMultilanguage, LanguageCulture>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMultilanguageQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LanguageCulture> Handle(GetMultilanguage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}