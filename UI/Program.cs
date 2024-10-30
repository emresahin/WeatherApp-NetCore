using Business.DataAccess;
using Business.DataAccess.Concrete;
using Business.DataAccess.Interfaces;
using Business.Services.Interfaces;
using Business.Services.Provider;
using Business.WeatherBusiness.Concrete;
using Business.WeatherBusiness.Interfaces;
using Core.Cache;
using Core.Cache.Extensions;
using Core.Cache.Provider;
using Core.DataAccess.Extensions;
using Core.ExceptionHandler;
using Core.HttpRequest;
using Core.HttpRequest.Extensions;
using Core.HttpRequest.Interfaces;
using Core.HttpRequest.Provider;
using DataAccess.DBContext;
using DataAccess.Statics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearch"])) // Elasticsearch URI
{
    IndexFormat = "serilog-index",
    AutoRegisterTemplate = true,
}).MinimumLevel.Information().CreateLogger();
Log.Information("Starting up the application");


builder.Host.UseSerilog(Log.Logger);
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddMemoryCache();
builder.Services.ServiceRequesterRegister(typeof(RestClientProvider));
builder.Services.RegisterCacheProvider(typeof(RedisCacheProvider));

builder.Services.AddScoped<IWeatherBusiness, WeatherBusiness>();

builder.Services.AddScoped<IWeatherProvider, WeatherStackProvider>();
builder.Services.AddScoped<IWeatherProvider, WeatherApiProvider>();

builder.Services.DBAccessRegister<WeatherDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DataAccess")));

builder.Services.AddScoped<RepositoryContextManager>();
builder.Services.AddScoped<ICityBusiness, CityBusiness>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDBContext>();

    var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();

    if (!appliedMigrations.Any())
    {
        await dbContext.Database.MigrateAsync();
        await ApplicationDbContextSeed.SeedCitiesAsync(dbContext);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");

}


app.UseDirectoryBrowser();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
