using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Properties.Model;

namespace Properties.Tests
{
    public class PropertiesControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly TestServer _testServer;


        public PropertiesControllerIntegrationTests()
        {
            var builder = new WebHostBuilder().ConfigureTest();

            _testServer = new TestServer(builder);
            _testClient = _testServer.CreateClient();

            using (var scope = _testServer.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                Seed(context);
            }
        }

        private void Seed(ApplicationDbContext context)
        {

        }



    }
}