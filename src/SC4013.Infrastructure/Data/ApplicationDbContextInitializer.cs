using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SC4013.Domain.Entities;

namespace SC4013.Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var provider = _configuration.GetValue("Provider", Provider.Sqlite.Name);
            if (provider == Provider.SqlServer.Name)
            {
                // SQL Server uses separate connection string for migrations
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(_configuration.GetConnectionString("MasterConnection"))
                    .Options;
                await using var dbContext = new ApplicationDbContext(options);
                await dbContext.Database.MigrateAsync();
            }
            else
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯" },
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}