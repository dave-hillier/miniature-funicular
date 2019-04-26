using System;
using System.Linq;
using HalHelper;
using Tasks.Model;

namespace Tasks.Resources
{
    static class ResourceFactory
    {
        public static Resource ToResource(this TaskList taskList)
        {
            var tasks = taskList.Tasks.Select(ToResource);
            return new Resource($"/api/lists/{taskList.Id}")
                {
                    State = taskList
                }
                .AddEmbedded("tasks", tasks
                    .OrderBy(t => ((TaskModel)t.State).Position)
                    .ToList());
        }

        public static Resource ToResource(this TaskModel taskModel)
        {
            var result = new Resource($"/api/tasks/{taskModel.Id}")
            {
                State = taskModel
            };
            if (taskModel.Children != null)
            {
                result.AddEmbedded("children", taskModel.Children
                    .OrderBy(t => t.Position)
                    .Select(ToResource)
                    .ToList());
            }

            return result;
        }
    }
    
}