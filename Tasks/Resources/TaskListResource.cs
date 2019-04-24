using System;
using System.Linq;
using HalHelper;
using Tasks.Model;

namespace Tasks.Resources
{
    class ResourceFactory
    {
        public static ResourceBase CreateTaskList(TaskList taskList)
        {
            var tasks = taskList.Tasks.Select(CreateTask);
            return new TaskListResource($"/api/lists/{taskList.Id}").AddEmbedded("tasks", tasks.ToList());
        }

        public static ResourceBase CreateTask(TaskModel taskModel)
        {
            var result = new TaskResource($"/api/tasks/{taskModel.Id}")
            {
                Due = taskModel.Due,
                Notes = taskModel.Notes,
                Completed = taskModel.Completed,
                Title = taskModel.Title,
                Status = taskModel.Status,
                Updated = taskModel.Updated,
                Position = taskModel.Position               
            };
            if (taskModel.Children != null)
            {
                result.AddEmbedded("children", taskModel.Children
                    .OrderBy(t => t.Position)
                    .Select(CreateTask)
                    .ToList());
            }

            return result;
        }
    }
    
    public class TaskListResource : ResourceBase
    {
        public TaskListResource(string selfLink) : base(selfLink)
        {
        }

        public string Title { get; set; }
        public DateTime Updated { get; set; }
    }
}