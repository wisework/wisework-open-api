using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.Authentication.Commands.Login;

public record LoginCommand : IRequest<AuthenticationInfo>
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationInfo>
{
    private readonly IApplicationDbContext _context;
    private readonly ICryptographyService _cryptographyService;
    private readonly ISecurityService _securityService;

    public LoginCommandHandler(IApplicationDbContext context, ICryptographyService cryptographyService, ISecurityService securityService)
    {
        _context = context;
        _cryptographyService = cryptographyService;
        _securityService = securityService;
    }

    public async Task<AuthenticationInfo> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var companyUsers = _context.DbSetCompanyUser.Where(cu => cu.Status == "A");

        var loginInfo = (from u in _context.DbSetUser
                        join cu in companyUsers on u.UserId equals cu.UserID
                        where u.Username == request.Username && u.Status == "A"
                        select new LoginModel
                        {
                            CompanyId = Convert.ToInt32(cu.CompanyID),
                            Email = u.Email,
                            Guid = u.Guid,
                            UserID = Convert.ToInt32(u.UserId),
                            Username = u.Username,
                            Version = u.Version,
                        }).FirstOrDefault();

        string password = "";

        if (loginInfo != null )
        {
            password = _cryptographyService.generatePassword(request.Password, loginInfo.Guid);
        }

        var authentication = (from u in _context.DbSetUser
                              join cu in companyUsers on u.UserId equals cu.UserID
                              where u.Username == request.Username && u.Password == password && u.Status == "A"
                              select new AuthenticationModel
                              {
                                  TokenID = Guid.NewGuid().ToString().ToUpper(),
                                  UserID = Convert.ToInt32(u.UserId),
                                  CompanyID = Convert.ToInt32(cu.CompanyID),
                                  TokenDate = DateTime.Now.AddDays(1),
                                  VisitorId = "",
                              }).FirstOrDefault();

        string jsonAuthentication = "";


        if (authentication != null)
        {
            jsonAuthentication = Newtonsoft.Json.JsonConvert.SerializeObject(authentication);
        }

        var token = _securityService.Encrypt(jsonAuthentication);
        var entity = new AuthenticationInfo { 
            Token = token
        };

        return entity;
    }
}
