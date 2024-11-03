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

        // services.AddAuthentication()
        //     .AddBearerToken(IdentityConstants.BearerScheme);

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

        

        //services.AddAuthorizationBuilder();

        // services.AddIdentityCore<ApplicationUser>()
        //     .AddRoles<IdentityRole>()
        //     .AddEntityFrameworkStores<IdentityContext>()
        //     .AddDefaultTokenProviders()
        //     .AddApiEndpoints();

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
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.RequireUniqueEmail = true;
        });
        
        // services.AddAuthentication(x =>
        // {
        //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
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

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization();

        return services;
    }
}