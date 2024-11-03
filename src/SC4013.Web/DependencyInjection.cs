using Microsoft.AspNetCore.Mvc;
using SC4013.Application.Common.Interfaces;
using SC4013.Web.Common;
using SC4013.Web.Services;

namespace SC4013.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();
        
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "SC4013 API";
        });

        return services;
    }
}