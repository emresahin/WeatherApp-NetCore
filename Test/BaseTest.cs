using Business.DataAccess.Concrete;
using Business.DataAccess.Interfaces;
using Business.DataAccess;
using Business.Services.Interfaces;
using Business.Services.Provider;
using Business.WeatherBusiness.Concrete;
using Business.WeatherBusiness.Interfaces;
using Core.Cache.Provider;
using Core.HttpRequest.Provider;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cache.Extensions;
using Core.HttpRequest.Extensions;
using Core.DataAccess.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Sinks.Elasticsearch;
using Serilog;
namespace Test
{
    public class BaseTest:IDisposable
    {
        public IServiceProvider ServiceProvider;

        [SetUp]
        public void BaseSetup()
        {
            IServiceCollection services = new ServiceCollection();


            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().MinimumLevel.Fatal().CreateLogger();

            services.AddLogging(l => { l.AddSerilog(); });
            
            
            // Konfigürasyon ayarlarını yükle
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(config);

            services.AddMemoryCache();

           

            services.RegisterCacheProvider(typeof(RedisCacheProvider));
            services.ServiceRequesterRegister(typeof(RestClientProvider));

            services.AddScoped<IWeatherBusiness, WeatherBusiness>();
            
            services.AddScoped<IWeatherProvider, WeatherStackProvider>();
            services.AddScoped<IWeatherProvider, WeatherApiProvider>();
            
            services.DBAccessRegister<WeatherDBContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<RepositoryContextManager>();
            services.AddScoped<ICityBusiness, CityBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            
        }
    }
}
