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

    public AuthenticationModel? GetAuthentication(string token)
    {
        if (string.IsNullOrEmpty(token) || !VerifyToken(token)) 
        {
            return null;
        }

        var authentication = DecryptToken(token);
        return authentication;
    }

    public bool VerifyToken(string token)
    {
        if(string.IsNullOrEmpty(token)) return false;

        var authentication = DecryptToken(token);
        return authentication != null && !IsTokenExpired(authentication.TokenDate);
        
    }

    public AuthenticationModel? DecryptToken(string token)
    {
        try
        {
            var jsonAuthentication = _securityService.Decrypt(token);
            var authentication = JsonConvert.DeserializeObject<AuthenticationModel>(jsonAuthentication);

            return authentication;
        }
        catch (JsonReaderException)
        {
            return null;
        }catch { throw new UnauthorizedAccessException(); }
    }

    public bool IsTokenExpired(DateTime tokenDate)
    {
        return tokenDate < DateTime.Now;
    }
}
