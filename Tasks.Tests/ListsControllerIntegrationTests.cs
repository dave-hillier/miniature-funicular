using System;
using System.Net;
using System.Net.Http;
using HalHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Tasks.Model;
using Tasks.Resources;
using Xunit;

namespace Tasks.Tests
{
    public class ListsControllerIntegrationTests
    {
        private readonly HttpClient _testClient;

        public ListsControllerIntegrationTests()
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
            var list = new TaskList
            {
                Tenant = "Tenant", 
                Title = "List",
                Id = "List1"
            };            
            context.List.Add(list);
            
            var task1 = new TaskModel
            {
                Id = "Task1",
                Tenant = "Tenant", 
                Title = "Title1",
                ParentTaskList = list
            };
            context.Tasks.Add(task1);
            
            var task2 = new TaskModel
            {
                Id = "Task2",
                Tenant = "Tenant", 
                Title = "Title2",
                Completed = DateTime.Today,
                Parent = task1
            };
            context.Tasks.Add(task2);
            
            context.SaveChanges();
        }

        [Fact]
        public async void GetAllLists()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ResourceBase>(responseBody);

            
        }

        [Fact]
        public async void GetList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<TaskListResource>(responseBody);
        }

        [Fact]
        public async void GetNotFoundList()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/lists/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}