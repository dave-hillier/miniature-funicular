using System;
using System.Net;
using System.Net.Http;
using HalHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Users.Model;
using Users.Resources;
using Xunit;

namespace Users.Tests
{
    public class GroupsControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly string _id1 = Guid.NewGuid().ToString();
        private readonly string _id2 = Guid.NewGuid().ToString();
        private readonly string _id3 = Guid.NewGuid().ToString();
        
        public GroupsControllerIntegrationTests()
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
            context.Users.Add(new User { Username = "User1", Id = _id1, Tenant = "Tenant" });
            context.Users.Add(new User { Username = "User2", Id = _id2, Tenant = "Tenant" });

            context.Groups.Add(new Group {DisplayName = "Admin", Id = _id3, Tenant = "Tenant"}); 
            context.Groups.Add(new Group {DisplayName = "Group1", Tenant = "Tenant"}); 
            context.Groups.Add(new Group {DisplayName = "Group2", Tenant = "Tenant"}); 
            
            context.SaveChanges();
        }

        [Fact]
        public async void GetGroupsList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/groups", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ResourceBase>(responseBody);

            Assert.Equal(3, resource.Embedded["data"].Count);

            Assert.Contains("Admin", responseBody);
            Assert.Contains("Group1", responseBody);
            Assert.Contains("Group2", responseBody);
        }
        
        [Fact]
        public async void GetGroup()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/groups/{_id3}", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<UserGroupResource>(responseBody);
            
            Assert.Equal("Admin", resource.DisplayName);
        }
        
        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/groups/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);           
        }
        // TODO: multi-tenancy
    }
}