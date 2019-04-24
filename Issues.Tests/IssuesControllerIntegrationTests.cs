﻿using System;
using System.Net;
using System.Net.Http;
using HalHelper;
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
        private const string Id1 = "1";
        private const string Id2 = "2";

        public IssuesControllerIntegrationTests()
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

            context.SaveChanges();
        }

        [Fact]
        public async void GetIssueList()
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
            var request = HttpClientHelper.CreateJsonRequest($"/api/Issues/{Id1}", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<IssueResource>(responseBody);
        }


        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/Issues/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}