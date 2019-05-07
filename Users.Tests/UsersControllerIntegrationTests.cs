using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Users.Model;

namespace Users.Tests
{
    public class UsersControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private const string Id1 = "1";
        private const string Id2 = "2";

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
            var group = new Group {DisplayName = "Admin", Id = "G", Tenant = "Tenant"};
            context.Groups.Add(group);
            
            var user = new User
            {
                Username = "Username", Id = Id1, Tenant = "Tenant",
            };
            var userGroup = new UserGroup
            {
                GroupId = group.Id,
                Group = group, 
                UserId = user.Id,
                User = user                    
            };
            user.UserGroups = new List<UserGroup> { userGroup };
            context.Users.Add(user);
            
            context.Users.Add(new User { Username = "User2", Id = Id2, Tenant = "Tenant" });
            
            context.Users.Add(new User { Username = "User1", Tenant = "OtherTenant" });

            context.UserGroups.Add(userGroup);
            
            context.SaveChanges();
        }

        [Fact]
        public async void GetUserList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/users", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("Username", responseBody);
            Assert.Contains("User2", responseBody);

            Assert.Contains($"/api/users/{Id1}", responseBody);
            Assert.Contains($"/api/users/{Id2}", responseBody);
        }

        [Fact]
        public async void GetUser()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/users/{Id1}", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("Username", responseBody);
            Assert.Contains("Admin", responseBody);
        }


        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/users/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async void GetMe()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/users/@me", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/api/users/1", response.Headers.Location.ToString());
            
        }
    }
}