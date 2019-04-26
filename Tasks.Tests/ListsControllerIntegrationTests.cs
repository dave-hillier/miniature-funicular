using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Model;
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
        public async void GetAllLists()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("List1", responseBody);
            Assert.Contains("Task1", responseBody);
            Assert.Contains("Task2", responseBody);
        }

        [Fact]
        public async void GetList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("List1", responseBody);
            Assert.Contains("Task1", responseBody);
            Assert.Contains("Task2", responseBody);
        }
        
        [Fact]
        public async void GetOrderedList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List2", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.True(responseBody.IndexOf("Task4", StringComparison.Ordinal) 
                        < responseBody.IndexOf("Task3", StringComparison.Ordinal));
        }

        [Fact]
        public async void GetNotFoundList()
        {
            var request = HttpClientHelper.CreateJsonRequest($"/api/lists/notFound", HttpMethod.Get, null);

            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async void DeleteList()
        {
            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Delete, null);
            var response = await _testClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var request2 = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }
        
        [Fact]
        public async void CreateList()
        {
            var payload = new 
            {
                Title = "New Title"
            };            

            var request = HttpClientHelper.CreateJsonRequest($"/api/lists", HttpMethod.Post, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location.ToString();
            Assert.Matches("/api/lists/.+", location);
            
            var request2 = HttpClientHelper.CreateJsonRequest(location, HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"New Title\"", responseBody);
        }
        
        [Fact]
        public async void UpdateList()
        {
            var payload = new 
            {
                Title = "New Title"
            };            

            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Put, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"New Title\"", responseBody);
        }
        
        [Fact]
        public async void PatchList()
        {
            var payload = new 
            {
                Title = "Patch Title"
            };            

            var request = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Patch, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"Patch Title\"", responseBody);
        }
        
        [Fact]
        public async void CreateTask()
        {
            var payload = new 
            {
                Title = "New Task on List1"
            };            

            var request = HttpClientHelper.CreateJsonRequest($"/api/lists/List1", HttpMethod.Post, payload);
            var response = await _testClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location.ToString();
            Assert.Matches("/api/tasks/.+", location);
            
            var request2 = HttpClientHelper.CreateJsonRequest("/api/lists/List1", HttpMethod.Get, null);
            var response2 = await _testClient.SendAsync(request2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            var responseBody = await response2.Content.ReadAsStringAsync();
            
            Assert.Contains("\"New Task on List1\"", responseBody);          
        }
        
        // TODO: task order
    }
}