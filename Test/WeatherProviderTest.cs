using Business.Services.Interfaces;
using Business.Services.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    public class WeatherProviderTest : BaseTest
    {
        private IEnumerable<IWeatherProvider> _providers;
        [SetUp]
        public void Setup()
        {
            _providers = ServiceProvider.GetRequiredService<IEnumerable<IWeatherProvider>>();
        }

        [Test]
        public async Task Test_SuccessWeatherApi()
        {
            var provider = _providers.OfType<WeatherApiProvider>().FirstOrDefault();
            var result = await provider.GetCurrentWeatherByCityAsync("Ankara");
            Assert.IsTrue(result > 0);

        }

        [Test]
        public async Task Test_ErrorWeatherApi()
        {
            var provider = _providers.OfType<WeatherApiProvider>().FirstOrDefault();
            var result = await provider.GetCurrentWeatherByCityAsync(string.Empty);
            Assert.IsTrue(result == 0);

        }

        [Test]
        public async Task Test_SuccessWeatherStack()
        {
            var provider = _providers.OfType<WeatherStackProvider>().FirstOrDefault();
            var result = await provider.GetCurrentWeatherByCityAsync("Ankara");
            Assert.IsTrue(result > 0);

        }

        [Test]
        public async Task Test_ErrorWeatherStack()
        {
            var provider = _providers.OfType<WeatherStackProvider>().FirstOrDefault();
            var result = await provider.GetCurrentWeatherByCityAsync(string.Empty);
            Assert.IsTrue(result == 0);
        }
    }
}