using System;
using IXBI.Net.Helper;
namespace SEMS.Service
{
    public class ConfigurationService
    {
        public static bool readEnvWebConfgFile()
        {
            string configFrom = ConfigurationHelper.GetAppSettings("EnvFrom");
            if (configFrom == "file")
            {
                return true;
            }
            return false;
        }

        public enum AppSettings
        {
            SmsApiKey,
            SmsSecretKey,
            SmsProvider,
            SmsBaseUrl,
            SmsSender,
            AwsSecretKey,
            AwsAccessKey,
            AWSBuckets,
            StorageProvider,
            IsosuiteAzureStorageConnectionString,
            IsosuiteAzureStorageContainer,
            Theme,
            DSRTheme,
            UrlDisplay,
            TemplateEmailDataBreach,
            stackkey
        }

        public static string getAppSettings(AppSettings appSetting)
        {
            if (!ConfigurationService.readEnvWebConfgFile())
            {
                string value = Environment.GetEnvironmentVariable(appSetting.ToString());
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return ConfigurationHelper.GetAppSettings(appSetting.ToString());

        }


        #region SqlConnection Configuration  Properties

        public static string SqlConnectionString
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SqlConnectionString");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SqlConnectionString");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public static int SqlCommandTimeout
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = (Environment.GetEnvironmentVariable("SqlCommandTimeout"));
                        if (!string.IsNullOrEmpty(value))
                            return NumberHelper.StringToInteger(value);
                    }
                    return NumberHelper.StringToInteger((ConfigurationHelper.GetAppSettings("SqlCommandTimeout")));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static int TransactionTimeout
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = ConfigurationHelper.GetAppSettings("TransactionTimeout");
                        if (!string.IsNullOrEmpty(value))
                            return NumberHelper.StringToInteger(value);

                    }
                    return NumberHelper.StringToInteger((Environment.GetEnvironmentVariable("TransactionTimeout")));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region OracleConnection Configuration  Properties

        public static string OracleConnectionString
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("OracleConnectionString");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("OracleConnectionString");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static int OracleCommandTimeout
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("OracleCommandTimeout");
                        if (!string.IsNullOrEmpty(value))
                            return NumberHelper.StringToInteger(value);
                    }
                    return NumberHelper.StringToInteger((ConfigurationHelper.GetAppSettings("OracleCommandTimeout")));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Ftp Configuration Properties

        public static string FtpUrl
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("FtpUrl");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("FtpUrl");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string FtpUserName
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("FtpUserName");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("FtpUserName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string FtpPassword
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("FtpPasswords");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("FtpPassword");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Mail Configuration Properties

        public static string MailServer
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("MailServer");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("MailServer");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string MailFrom
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("MailFrom");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("MailFrom");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string MailFromDisplayName
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("MailFromDisplayName");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("MailFromDisplayName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string MailUsername
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("MailUsername");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("MailUsername");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string MailPassword
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("MailPassword");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("MailPassword");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Azure Storage Configuration Properties



        public static string IsosuiteAzureStorageUrl
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("IsosuiteAzureStorageUrl");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("IsosuiteAzureStorageUrl");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        #endregion

        #region Security Configuration  Properties

        public static string SecuredPassword
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SecuredPassword");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SecuredPassword");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static int TokenTimeout
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("TokenTimeout");
                        if (!string.IsNullOrEmpty(value))
                            return NumberHelper.StringToInteger(value);
                    }
                    return NumberHelper.StringToInteger((ConfigurationHelper.GetAppSettings("TokenTimeout")));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string AllowedDomains
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("AllowedDomains");
                        if (!string.IsNullOrEmpty(value))
                            return value;

                    }
                    return ConfigurationHelper.GetAppSettings("AllowedDomains");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Data Configuration  Properties

        public static string EncodingFormat
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("EncodingFormat");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("EncodingFormat");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static string CustomerDirectory
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("CustomerDirectory");
                        if (!string.IsNullOrEmpty(value))
                            return value;

                    }
                    return ConfigurationHelper.GetAppSettings("CustomerDirectory");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static string ProfileDirectory
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("ProfileDirectory");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("ProfileDirectory");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static string DocumentsDirectory
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("DocumentsDirectory");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("DocumentsDirectory");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        #endregion

        #region SendGrid Configuration Properties

        public static string SendGridKey
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SendGridKey");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SendGridKey");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string SmsBaseUrl
        {
            get
            {
                try
                {

                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SmsBaseUrl");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SmsBaseUrl");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string SmsApiKey
        {
            get
            {
                try
                {

                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SmsApiKey");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SmsApiKey");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string SmsSecretKey
        {
            get
            {
                try
                {

                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("SmsSecretKey");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("SmsSecretKey");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string EmailFromAddress
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("EmailFromAddress");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("EmailFromAddress");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static string ENV
        {
            get
            {
                try
                {

                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("environment");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("environment");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string DeviceManager
        {
            get
            {
                try
                {

                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("deviceManager");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("deviceManager");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public static string WebApiUrl
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("WebApiUrl");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("WebApiUrl");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string DomainName
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("DomainName");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("DomainName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string GenerateURLService
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("GenerateURLService");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("GenerateURLService");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public static string BitlyPassword
        {
            get
            {
                try
                {
                    if (!ConfigurationService.readEnvWebConfgFile())
                    {
                        string value = Environment.GetEnvironmentVariable("BitlyPassword");
                        if (!string.IsNullOrEmpty(value))
                            return value;
                    }
                    return ConfigurationHelper.GetAppSettings("BitlyPassword");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        #endregion



    }
}
