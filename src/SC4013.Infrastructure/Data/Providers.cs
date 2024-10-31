namespace SC4013.Infrastructure.Data;

public record Provider(string Name, string Assembly)
{
    public static readonly Provider Sqlite = new(nameof(Sqlite), "SC4013.SqliteMigrations");
    
    public static readonly Provider SqlServer = new(nameof(SqlServer), "SC4013.SqlServerMigrations");
}