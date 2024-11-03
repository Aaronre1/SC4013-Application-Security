using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC4013.Domain.Constants;
using SC4013.Infrastructure.Identity;

namespace SC4013.Infrastructure.Data;

public class IdentityDbContextInitializer
{
    private readonly ILogger<IdentityDbContextInitializer> _logger;
    private readonly IdentityContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityDbContextInitializer(
        ILogger<IdentityDbContextInitializer> logger,
        IdentityContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Seed roles
        var adminRole = new IdentityRole(Roles.Admin);
        var leaderRole = new IdentityRole(Roles.Leader);

        if (_roleManager.Roles.All(x => x.Name != adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }
        if (_roleManager.Roles.All(x => x.Name != leaderRole.Name))
        {
            await _roleManager.CreateAsync(leaderRole);
        }
        
        // Seed users
        var defaultPassword = "Password1234567890!";
        var adminUser = new ApplicationUser
        {
            UserName = "admin@localhost",
            Email = "admin@localhost"
        };
        
        if (_userManager.Users.All(u => u.UserName != adminUser.UserName))
        {
            await _userManager.CreateAsync(adminUser, defaultPassword);
            if (!string.IsNullOrWhiteSpace(adminRole.Name))
            {
                await _userManager.AddToRolesAsync(adminUser, new [] { adminRole.Name });
            }
        }
        
        var leaderUser = new ApplicationUser
        {
            UserName = "leader@localhost",
            Email = "leader@localhost"
        };
        
        if (_userManager.Users.All(u => u.UserName != leaderUser.UserName))
        {
            await _userManager.CreateAsync(leaderUser, defaultPassword);
            if (!string.IsNullOrWhiteSpace(leaderRole.Name))
            {
                await _userManager.AddToRolesAsync(leaderUser, new [] { leaderRole.Name });
            }
        }
        
    }
}