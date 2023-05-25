using Microsoft.Extensions.Configuration;

namespace WW.Infrastructure.Services.Upload.Infrastructures;
public static class ConfigurationFactory
{
    private static IConfiguration _configuration;
    public static void Create(IConfiguration configuration)
    {
        ConfigurationFactory._configuration = configuration;
    }
    public static IConfiguration GetConfigApp()
    {
        return ConfigurationFactory._configuration as IConfiguration;
    }

}
