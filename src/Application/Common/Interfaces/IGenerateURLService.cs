using WW.Application.Common.Models;

namespace WW.Application.Common.Interfaces;
public interface IGenerateURLService
{
    Task<GenerateURLModel> GenerateShortenURL(GenerateURLModel url);
}

