using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;


namespace WW.Application.User.Queries.GetUser;


public record GetUserQuery(long id) : IRequest<UserInfo>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserInfo>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserInfo> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Users, UserInfo>();
            cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
        });

        Mapper mapper = new Mapper(config);

        var userInfo = (from cf in _context.DbSetUser
                               where cf.UserId == request.id && cf.Status != Status.X.ToString()
                               select new UserInfo
                               {
                                   Address = cf.Address,
                                   BirthDate = cf.BirthDate.ToString(),
                                   CitizenId = cf.CitizenId,
                                   CompanyId = cf.CompanyId,
                                   Email = cf.Email,
                                   FullName = $"{cf.FirstName} {cf.LastName}",
                                   Guid = new Guid(cf.Guid),
                                   Gender = cf.Gender,
                                   PositionDescription = cf.PositionDescription,
                                   ProfileImage = cf.ProfileImage,
                                   Tel = cf.Tel,
                                   UserID = Convert.ToInt32(cf.UserId),
                                   Username = cf.Username,
                                   Version = cf.Version,
                               }).FirstOrDefault();

        if (userInfo == null)
        {
            return new UserInfo();
        }

        return userInfo;
    }
}
