using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Tasks.Model;

namespace Tasks.Tests
{
    public class TasksControllerIntegrationTests
    {
        private readonly HttpClient _testClient;

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

        private static void Seed(ApplicationDbContext context)
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
                ParentTaskList = list
            };
            context.Tasks.Add(task1);
            
            var task2 = new TaskModel
            {
                Id = "Task2",
                Tenant = "Tenant", 
                Parent = task1
            };
            context.Tasks.Add(task2);
            
            var list2 = new TaskList
            {
                Tenant = "Tenant", 
                Title = "List2",
                Id = "List2"
            };            
            context.List.Add(list2);
            var task3 = new TaskModel
            {
                Id = "Task3",
                Tenant = "Tenant", 
                ParentTaskList = list2,
                Position = "b"
            };
            context.Tasks.Add(task3);
            var task4 = new TaskModel
            {
                Id = "Task4",
                Tenant = "Tenant", 
                ParentTaskList = list2,
                Position = "a"
            };
            context.Tasks.Add(task4);
            
            context.SaveChanges();
        }

        [Fact]
        public async void GetTask()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal("{\"_links\":{\"self\":{\"href\":\"/api/tasks/Task1\"}},\"_embedded\":{\"children\":[{\"_links\":{\"self\":{\"href\":\"/api/tasks/Task2\"}}}]}}", 
                responseBody);
        }
        
        [Fact]
        public async void DeleteTask()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Delete, null);
            var response = await _testClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var request2 = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }

        [Fact]
        public async void GetNotFound()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/tasks/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
        
        [Fact]
        public async void UpdateTask()
        {
            var payload = new 
            {
                Title = "New Title"
            };            

            var request = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Put, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"New Title\"", responseBody);
        }
        
        [Fact]
        public async void PatchTask()
        {
            var payload = new 
            {
                Title = "Patch Title"
            };            

            var request = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Patch, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"Patch Title\"", responseBody);
        }
        
        [Fact]
        public async void CreateSubTask()
        {
            var payload = new 
            {
                Title = "New Task on Task1"
            };            

            var request = HttpClientHelper.CreateJsonRequest($"/api/tasks/Task1", HttpMethod.Post, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location.ToString();
            Assert.Matches("/api/tasks/.+", location);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/tasks/Task1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"New Task on Task1\"", responseBody);          
        }

    }
}