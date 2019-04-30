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

        public HealthCheckIntegrationTests()
        {
            var builder = new WebHostBuilder().ConfigureTest();

            var testServer = new TestServer(builder);
            _testClient = testServer.CreateClient();
        }

        [Fact]
        public async void GetHealthReturnsOk()
        {
            var response = await _testClient.GetAsync("/status/health");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
