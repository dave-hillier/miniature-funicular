﻿using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Issues.Model;

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
                Description = "Description1",
                Category = "Test"
            });
            context.Add(new Issue
            {
                Tenant = "Tenant",
                Id = "Issue2",
                Description = "Description2",
                Category = "Test"
            });
            context.SaveChanges();
        }
       
        [Fact]
        public async void GetAllIssues()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/issues", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("\"description\":\"Description2\"", responseBody);
            Assert.Contains("\"description\":\"Description1\"", responseBody);
        }

        [Fact]
        public async void GetIssue()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("\"description\":\"Description1\"", responseBody);
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

        /*[Fact]
        public async void CreateIssue()
        {
            var payload = new 
            {
                Description = "New Issue"
            };            

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues/resource", HttpMethod.Post, payload);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location.ToString();
            Assert.Matches("/api/issues/.+", location);
            
        }*/
        // TODO: form create

        [Fact]
        public async void UpdateIssue()
        {
            var payload = new 
            {
                Description = "New Description",                
            };

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues/Issue1", HttpMethod.Put, payload);

            var response = await _testClient.SendAsync(request);
            
            var request1 = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);

            var response1 = await _testClient.SendAsync(request1);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var responseBody = await response1.Content.ReadAsStringAsync();
            Assert.Contains("\"description\":\"New Description\"", responseBody);
            Assert.DoesNotContain("Test", responseBody);
            
        }
        
        [Fact]
        public async void PatchIssue()
        {
            var payload = new 
            {
                Description = "New Description",
                
            };

            var request = HttpClientHelper.CreateJsonRequest($"/api/issues/Issue1", HttpMethod.Patch, payload);

            var patchResponse = await _testClient.SendAsync(request);
            

            var request1 = HttpClientHelper.CreateJsonRequest($"/api/Issues/Issue1", HttpMethod.Get, null);
            var getResponse = await _testClient.SendAsync(request1);

            var responseBody = await getResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
            
            Assert.Contains("\"description\":\"New Description\"", responseBody);
            Assert.Contains("Test", responseBody);
        }
        

    }
}