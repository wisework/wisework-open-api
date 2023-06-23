using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WW.Application.Common.Interfaces;
using WW.Infrastructure.Files;
using WW.Infrastructure.Identity;
using WW.Infrastructure.Persistence;
using WW.Infrastructure.Persistence.Interceptors;
using WW.Infrastructure.Services;
using WW.Infrastructure.Services.Authentication;

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
        UploadFactory.Create(configuration);
        services.AddTransient<IUploadService>(x =>
        {
            return UploadFactory.Create(configuration);
        });
        services.AddTransient<IGenerateURLService, GenerateURLService>();
        services.AddTransient<ICryptography, CryptographyService>();
        services.AddTransient<ISecurityService, SecurityService>();
        services.AddAuthentication();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
