using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WW.Application.Common.Models;

namespace WW.Application.Common.Interfaces;
public interface IGenerateURLService
{
    Task<GenerateURLModel> GenerateShortenURL(GenerateURLModel url);
}

