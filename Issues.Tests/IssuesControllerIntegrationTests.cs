using System;
using System.Net;
using System.Net.Http;
using HalHelper;
using Issues.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Newtonsoft.Json;
using Issues.Model;
using Issues.Resources;

namespace Issues.Tests
{
    public class IssuesControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly TestServer _testServer;


        public IssuesControllerIntegrationTests()
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
            context.Add(new Issue
            {
                Tenant = "Tenant",
                Id = "Issue1",
                Title = "Title1",
                Description = "Description1"
            });
            context.Add(new Issue
            {
                Tenant = "Tenant",
                Id = "Issue2",
                Title = "Title2",
                Description = "Description2"
            });
            context.SaveChanges();
        }

        [Fact]
        public void DatabaseShit()
        {
            using (var scope = _testServer.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var issue = context.Issues.Find("Issue1");
                
                Assert.Equal("Title1", issue.Title);
                
                
            }
            
        }

        [Fact]
        public async void GetAllIssues()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/issues", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ResourceBase>(responseBody);
        }

        [Fact]
        public async void GetIssue()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal("{\"title\":\"Title1\",\"description\":\"Description1\",\"_links\":{\"self\":{\"href\":\"/api/issues/Issue1\"}},\"_embedded\":{}}", responseBody);
        }


        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/Issues/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void DeleteIssue()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/issues/Issue1", HttpMethod.Delete, null);
            var response = await _testClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var request2 = HttpClientHelper.CreateJsonRequest("/api/issues/Issue1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }

        [Fact]
        public async void CreateIssue()
        {
            var payload = new 
            {
                Title = "x"
            };

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues", HttpMethod.Post, payload);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location.ToString();
            Assert.Matches("/api/issues/.+", location);
            
        }

        [Fact]
        public async void UpdateIssue()
        {
            var payload = new 
            {
                Title = "New Title"
            };

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues/Issue1", HttpMethod.Put, payload);

            var response = await _testClient.SendAsync(request);
            
            var request1 = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);

            var response1 = await _testClient.SendAsync(request1);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var responseBody = await response1.Content.ReadAsStringAsync();
            Assert.Contains("\"title\":\"New Title\"", responseBody);
            Assert.DoesNotContain("Description1", responseBody);
            
        }
        
        [Fact]
        public async void PatchIssue()
        {
            var payload = new 
            {
                Title = "New Title"
            };

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues/Issue1", HttpMethod.Patch, payload);

            var response = await _testClient.SendAsync(request);
            
            var request1 = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);

            var response1 = await _testClient.SendAsync(request1);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var responseBody = await response1.Content.ReadAsStringAsync();
            Assert.Contains("\"title\":\"New Title\"", responseBody);
            Assert.Contains("Description1", responseBody);
        }
        

    }
}