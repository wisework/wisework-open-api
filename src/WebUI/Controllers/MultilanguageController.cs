using System.Web.Http;
using MediatR;
using WW.Application.Language;
using WW.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Wiskwork.OpenAPI.Controllers;
public class MultilanguageController : ApiController
{
    public IHttpActionResult GetLocalStringResourceList(List<LanguageSearch> languageSearchesList)
    {
        try
        {
            MultilanguageService multilanguageService = new MultilanguageService();

            List<LocalStringResourceDisplay> localStringResourceDisplaysList = multilanguageService.GetLocalStringResourceList(languageSearchesList);
            // get all LanguageCulture
            HashSet<string> languageCultureList = new HashSet<string>(localStringResourceDisplaysList.Select(dt => dt.LanguageCulture).ToList());

            Dictionary<string, object> localStringResourceKeyValuePairs = new Dictionary<string, object>();

            foreach (string item in languageCultureList)
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                foreach (LocalStringResourceDisplay entry in localStringResourceDisplaysList)
                {
                    if (item.Trim() == entry.LanguageCulture.Trim())
                    {
                        bool keyExists = keyValuePairs.ContainsKey(entry.ResourceKey.Trim());
                        if (!keyExists)
                        {
                            keyValuePairs.Add(entry.ResourceKey.Trim(), entry.ResourceValue.Trim());
                        }

                        localStringResourceKeyValuePairs[entry.LanguageCulture.Trim()] = keyValuePairs;
                    }
                }
            }
           
            return this.Ok(localStringResourceKeyValuePairs);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // GET: ListLanguage
    [System.Web.Http.Route("api/Multilanguage/GetLanguageList")]
    [System.Web.Http.AcceptVerbs("POST")]
    public IHttpActionResult GetLanguageList()
    {
        try
        {
            MultilanguageService multilanguageService = new MultilanguageService();
            List<LanguageDisplay> languageDisplaysList = multilanguageService.GetLanguageList();

            return this.Ok(languageDisplaysList);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // GET: ListLanguage
    [System.Web.Http.Route("api/Multilanguage/GetLanguageList/{id}")]
    [System.Web.Http.AcceptVerbs("POST")]
    public IHttpActionResult GetLanguageListByCollectionPointId(int id)
    {
        try
        {
            MultilanguageService multilanguageService = new MultilanguageService();
            List<LanguageDisplay> languageDisplaysList = multilanguageService.GetLanguageListByCollectionPointId(id);

            return this.Ok(languageDisplaysList);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
