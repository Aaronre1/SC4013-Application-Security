using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SC4013.Application.Common.Interfaces;
using SC4013.Infrastructure.Data;
using SC4013.Infrastructure.Data.Interceptors;
using SC4013.Infrastructure.Identity;
using static SC4013.Infrastructure.Data.Provider;

namespace SC4013.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditEntityInterceptor>();
        
        var provider = config.GetValue("Provider", Sqlite.Name);

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            if (provider == Sqlite.Name)
            {
                options.UseSqlite("Data Source=app.db", x => x.MigrationsAssembly(Sqlite.Assembly));
            }
            
            if (provider == SqlServer.Name)
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(SqlServer.Assembly));
            }
            
        }).AddScoped<IApplicationDbContext>(p => p.GetRequiredService<ApplicationDbContext>());

        services.AddDbContext<IdentityContext>((sp, options) =>
        {
            if (provider == Sqlite.Name)
            {
                options.UseSqlite("Data Source=identity.db", x => x.MigrationsAssembly(Sqlite.Assembly));
            }
            
            if (provider == SqlServer.Name)
            {
                var connectionString = config.GetConnectionString("IdentityConnection");
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(SqlServer.Assembly));
            }
        });

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<IdentityDbContextInitializer>();

        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();
        
        // Configure identity options
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 16;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 1;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
            // Microsoft Security Compliance Toolkit (SCT) baseline recommended value is 10.
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;
            options.User.RequireUniqueEmail = true;
        });

        services.ConfigureApplicationCookie(x =>
        {
            x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization();

        return services;
    }
}