using FluentAssertions;
using FooApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FooApiTests
{
    [TestFixture]
    public class WeatherForecastControllerTest
    {
        [Test]
        public async Task WeatherForecastControllerTestAsync()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseStartup<Startup>();
                    webHost.UseConfiguration(configuration);
                    webHost.UseTestServer();
                    webHost.ConfigureTestServices(services =>
                    {
                        services.AddLogging(config => config.AddConsole());      // can add any logger(s)
                    });
                });
            
            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync("WeatherForecast");
            var items = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            items.Should().NotBeEmpty();
        }
        
    }
}