using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SC4013.Infrastructure.Data;

public static class DbInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var initializer = scope.ServiceProvider.GetRequiredService<IdentityDbContextInitializer>();
        await initializer.InitializeAsync();
        await initializer.TrySeedAsync();
        
        var appInitializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await appInitializer.InitializeAsync();
        await appInitializer.TrySeedAsync();
    }
}