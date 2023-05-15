using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Application.CustomField.Queries.GetCustomField;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Users.Queries.GetUser;

public record GetUserQuery : IRequest<UserInfo>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserInfo>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<UserInfo> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    //public async Task<UserInfo> Handle(GetUserQuery request, CancellationToken cancellationToken)
    //{
    //    MapperConfiguration config = new MapperConfiguration(cfg =>
    //    {
    //        cfg.CreateMap<User, UserInfo>()
    //        ;
    //    });

    //    Mapper mapper = new Mapper(config);


    //    UserInfo model =
    //        await _context.DbSetUser.Select(user => user.UserId == 1)
    //                                .ProjectTo<User>(_mapper.ConfigurationProvider);

    //    return model;
    //}
}

