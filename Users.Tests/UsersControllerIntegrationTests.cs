using System;
using System.Net;
using System.Net.Http;
using HalHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Newtonsoft.Json;
using Users.Model;
using Users.Resources;

namespace Users.Tests
{
    public class UsersControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly string _id1 = Guid.NewGuid().ToString();
        private readonly string _id2 = Guid.NewGuid().ToString();
        
        public UsersControllerIntegrationTests()
        {
            var builder = new WebHostBuilder().ConfigureTest();

            var testServer = new TestServer(builder);
            _testClient = testServer.CreateClient();

            using (var scope = testServer.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                Seed(context);
            }
        }

        private void Seed(ApplicationDbContext context)
        {
            context.Users.Add(new User { Username = "User1", Id = _id1 });
            context.Users.Add(new User { Username = "User2", Id = _id2 });
            context.SaveChanges();
        }

        [Fact]
        public async void GetUserList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/users", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ResourceBase>(responseBody);

            Assert.Equal(2, resource.Embedded["data"].Count);

            Assert.Contains("User1", responseBody);
            Assert.Contains("User2", responseBody);
            
            Assert.Contains($"/api/users/{_id1}", responseBody);
            Assert.Contains($"/api/users/{_id2}", responseBody);
        }

        [Fact]
        public async void GetUser()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/users/{_id1}", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<UserResource>(responseBody);
            
            Assert.Equal("User1", resource.DisplayName);
        }
        
        
        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/users/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
           
        }
    }
}