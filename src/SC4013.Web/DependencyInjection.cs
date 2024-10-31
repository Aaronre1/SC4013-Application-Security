using SC4013.Application.Common.Interfaces;
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
        
        services.AddEndpointsApiExplorer();

        return services;
    }
}