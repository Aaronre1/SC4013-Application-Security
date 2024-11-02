## 🚀 Setup

### Adding Migrations  
Support for 2 providers: SQL Server and SQLite.  


#### SQL Server 
For SQL Server provider, use `SC4013.SqlServerMigrations` project.

1. Update connection string values in usersecrets.
2. Run command to add migration for `ApplicationDbContext`.
```powershell
dotnet ef migrations add <Migration Name> --startup-project ./src/SC4013.Web --project ./src/SC4013.SqlServerMigrations --context ApplicationDbContext -- --provider SqlServer
```
3. Run command to add migration for `IdentityContext`.
```powershell
dotnet ef migrations add <Migration Name> --startup-project ./src/SC4013.Web --project ./src/SC4013.SqlServerMigrations --output-dir IdentityMigrations --context IdentityContext -- --provider SqlServer
```

#### SQLite
For SQLite provider, user `SC4013.SqliteMigrations` project.

1. Run command to add migration for `ApplicationDbContext`.
```powershell
dotnet ef migrations add Initial --startup-project ./src/SC4013.Web --project ./src/SC4013.SqliteMigrations --context ApplicationDbContext -- --provider Sqlite
```
2. Run command to add migration for `IdentityContext`.
```powershell
dotnet ef migrations add Initial --startup-project ./src/SC4013.Web --project ./src/SC4013.SqliteMigrations --output-dir IdentityMigrations --context IdentityContext -- --provider Sqlite
```


## Issues

### On macOS, "There was an error exporting the HTTPS developer certificate to a file."  
[Solution](https://developercommunity.visualstudio.com/t/VS2022-Angular-ASPNET-Core-Template-out/10767300?sort=active&type=idea)
