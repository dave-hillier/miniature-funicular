using Xunit;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;

namespace Issues.Tests
{

    public class HealthCheckIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly TestServer _testServer;

        public HealthCheckIntegrationTests()
        {
            var inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
            var builder = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseSetting("Authentication:Audience", "https://audience")
                .UseSetting("Authentication:Authority", "https://authority")
                .ConfigureServices(services =>
                {
                    services.ConfigureInMemoryDatabases(inMemoryDatabaseRoot);
                    services.ConfigureUnvalidatedAuth();
                })
                .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            _testClient = _testServer.CreateClient();
        }

        [Fact]
        public async void GetHealthReturnsOk()
        {
            var response = await _testClient.GetAsync("/status/health");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
