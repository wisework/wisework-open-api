using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;

namespace WW.Infrastructure.Services.Authentication;
public class AuthenticationService : IAuthenticationService
{
    private readonly ISecurityService _securityService = new SecurityService();

    public bool VerifyToken(string token)
    {
        if(string.IsNullOrEmpty(token)) return false;

        try
        {
            var jsonAuthentication = _securityService.Decrypt(token);
            var authentication = JsonConvert.DeserializeObject<AuthenticationModel>(jsonAuthentication);

            return authentication != null && !IsTokenExpired(token);
        }catch (JsonReaderException)
        {
            return false;
        }
        
    }

    public bool IsTokenExpired(string token)
    {
        var jsonAuthentication = _securityService.Decrypt(token);
        var authentication = JsonConvert.DeserializeObject<AuthenticationModel>(jsonAuthentication);

        return authentication?.TokenDate < DateTime.Now;
    }
}
