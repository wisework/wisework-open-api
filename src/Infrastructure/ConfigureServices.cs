using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WW.Application.Common.Interfaces;
using WW.Infrastructure.Files;
using WW.Infrastructure.Persistence;
using WW.Infrastructure.Persistence.Interceptors;
using WW.Infrastructure.Services;
using WW.Infrastructure.Services.Authentication;
using UploadModule = Wisework.UploadModule;
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("WWDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            /*services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(@"Server=LAPTOP-740HLHKM\SQLEXPRESS;Initial Catalog=PDPA-DEV;Integrated Security = true"));*/
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IDateTime, DateTimeService>();

        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        services.AddSingleton<UploadModule.Interfaces.IUploadProvider>(service =>
        {
            return new UploadModule
                .UploadFactory()
                .CreateUploadProvider(new UploadModule.Models.Configuration
                {
                    ProviderName = configuration.GetValue<String>("Storage:Provider"),
                    SecretKey = configuration.GetValue<String>("Storage:SecretKey"),
                    BucketName = configuration.GetValue<String>("Storage:BucketName"),
                });
        });


        services.AddTransient<IGenerateURLService, GenerateURLService>();
        services.AddTransient<WW.Application.Common.Interfaces.IAuthenticationService, WW.Infrastructure.Services.Authentication.AuthenticationService>();
        services.AddTransient<ICryptographyService, CryptographyService>();
        services.AddTransient<ISecurityService, SecurityService>();
        services.AddAuthentication();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
