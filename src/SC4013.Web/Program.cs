using SC4013.Application;
using SC4013.Infrastructure;
using SC4013.Infrastructure.Data;
using SC4013.Web;
using SC4013.Web.Common;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(config);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseRateLimiter();
app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();

