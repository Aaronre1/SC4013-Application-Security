using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SC4013.Application.Common.Interfaces;
using SC4013.Infrastructure.Data;
using SC4013.Infrastructure.Identity;
using static SC4013.Infrastructure.Data.Provider;

namespace SC4013.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        var provider = config.GetValue("Provider", Sqlite.Name);

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
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

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(x =>
        //     {
        //         x.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidateIssuerSigningKey = true,
        //             ValidIssuer = config["JwtSettings:Issuer"],
        //             ValidAudience = config["JwtSettings:Audience"],
        //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
        //         };
        //     });

        // services.AddAuthentication(x =>
        // {
        //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        // }).AddJwtBearer(x =>
        // {
        //     x.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = config["JwtSettings:Issuer"],
        //         ValidAudience = config["JwtSettings:Audience"],
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
        //     };
        // });

        services.AddAuthorizationBuilder();

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();


        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization();

        return services;
    }
}