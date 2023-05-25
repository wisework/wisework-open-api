using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WW.Domain.Entities;
using IXBI.Net.Access;
using System.Runtime.Caching;
using SEMS.Service;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WW.Application.Language;
public class MultilanguageService
{
    private readonly MemoryCache cache = MemoryCache.Default;
    public List<LocalStringResourceDisplay> GetLocalStringResourceList(List<LanguageSearch> languageSearchesList)
    {
        try
        {
            List<LocalStringResourceDisplay> localStringResourceDisplaysList = new List<LocalStringResourceDisplay>();

            localStringResourceDisplaysList = (List<LocalStringResourceDisplay>)this.cache.Get("LOCAL_STRINGRE_SOURCE");
            if (localStringResourceDisplaysList == null)
            {
                SqlAccess dbAccess = new SqlAccess(ConfigurationService.SqlConnectionString, ConfigurationService.SqlCommandTimeout);
                string commandQuery = $"SELECT * FROM[LocalStringResource]";

                if (languageSearchesList != null)
                {
                    // กรณีเรียกแบบส่ง รายการภาษามา
                    if (languageSearchesList.Count > 0)
                    {
                        HashSet<string> resultLanguage = new HashSet<string>(languageSearchesList.Select(l => l.LanguageCulture).ToList());

                        commandQuery = string.Format("SELECT * FROM [LocalStringResource] WHERE LanguageCulture IN ('{0}')", string.Join("', '", resultLanguage));
                    }
                }

                using (SqlCommand command = new SqlCommand(commandQuery))
                {
                    localStringResourceDisplaysList = dbAccess.GetDataList<LocalStringResourceDisplay>(command);
                }

                DateTimeOffset policy = new CacheItemPolicy().AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60.0);
                this.cache.Set("LOCAL_STRINGRE_SOURCE", localStringResourceDisplaysList, policy);
            }
            
            return localStringResourceDisplaysList;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public List<LanguageDisplay> GetLanguageList()
    {
        try
        {
            List<LanguageDisplay> languageDisplaysList = new List<LanguageDisplay>();
            SqlAccess dbAccess = new SqlAccess(ConfigurationService.SqlConnectionString, ConfigurationService.SqlCommandTimeout);
            string commandQuery = $"SELECT [LanguageID],[Name],[Code],[LanguageCulture],[IconUrl],[DisplayOrder],[IsDefault] FROM [Language]";
            using (SqlCommand command = new SqlCommand(commandQuery))
            {
                languageDisplaysList = dbAccess.GetDataList<LanguageDisplay>(command);
            }

            return languageDisplaysList.OrderBy(l => l.DisplayOrder).ToList();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public List<LanguageDisplay> GetLanguageListByCollectionPointId(int id)
    {
        try
        {
            List<LanguageDisplay> languageDisplaysList = new List<LanguageDisplay>();
            SqlAccess dbAccess = new SqlAccess(ConfigurationService.SqlConnectionString, ConfigurationService.SqlCommandTimeout);

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = @"SELECT C.[PageID]
                            ,C.[CollectionPointID]
                            ,L.[LanguageID]
                            ,L.[Version]
                            ,L.[Status]
                            ,L.[CreateBy]
                            ,L.[CreateDate]
                            ,L.[UpdateBy]
                            ,L.[UpdateDate]
                            ,L.[Code]
                            ,L.[Name]
                            ,L.[LanguageCulture]
                            ,L.[IconUrl]
                            ,L.[CompanyID]
                            ,L.[DisplayOrder]
                            ,L.[IsDefault]
                            FROM [Consent_Page] AS C
                        LEFT JOIN [Language] L ON C.LanguageCulture = L.LanguageCulture
                        where C.CollectionPointID = @CollectionPointId";
                command.Parameters.Add(new SqlParameter("@CollectionPointId", id));
                command.CommandType = CommandType.Text;

                languageDisplaysList = dbAccess.GetDataList<LanguageDisplay>(command);
            }

            return languageDisplaysList.OrderBy(l => l.DisplayOrder).ToList();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public ResultDisplay UpdateSwitchLanguageUser(LanguageSearch multilanguage, Authentication authentication)
    {
        try
        {
            SqlAccess dbAccess = new SqlAccess(ConfigurationService.SqlConnectionString, ConfigurationService.SqlCommandTimeout);

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = @"
                                            DECLARE @RS INT

                                            UPDATE [User] SET [DefaultLanguageID] = @LanguageID WHERE [UserID] = @UserID; 

                                            SET @RS = 1 -- TRUE
                                            SELECT @RS AS [RS]";
                command.Parameters.Add(new SqlParameter("@LanguageID", multilanguage.LanguageID));
                command.Parameters.Add(new SqlParameter("@UserID", authentication.UserID));
                command.CommandType = CommandType.Text;

                ResultDisplay result = dbAccess.GetDataObject<ResultDisplay>(command);

                return result;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
