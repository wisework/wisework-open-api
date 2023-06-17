using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Interfaces;
public interface IAuthenticationService
{
    bool VerifyToken(string token);
}
