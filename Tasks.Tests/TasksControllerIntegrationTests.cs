using System;
using System.Net;
using System.Net.Http;
using HalHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Newtonsoft.Json;
using Tasks.Model;
using Tasks.Resources;

namespace Tasks.Tests
{
    public class TasksControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private const string Id1 = "1";
        private const string Id2 = "2";

        public TasksControllerIntegrationTests()
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
        public async void GetTaskList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/tasks", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ResourceBase>(responseBody);

            Assert.Equal(2, resource.Embedded["data"].Count);

            Assert.Contains("Task1", responseBody);
            Assert.Contains("Task2", responseBody);

            Assert.Contains($"/api/Tasks/{Id1}", responseBody);
            Assert.Contains($"/api/Tasks/{Id2}", responseBody);
        }

        [Fact]
        public async void GetTask()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/tasks/{Id1}", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<TaskResource>(responseBody);

        }


        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/tasks/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}