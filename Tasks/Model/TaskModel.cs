using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Tasks.Model
{
    public class TaskModel
    {
        [JsonIgnore]
        public string Id { get; set; }
               
        [MaxLength(20)]
        [JsonIgnore]
        public string Tenant { get; set; }
        
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public string Position { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; } // TODO: enum?

        public DateTime? Due { get; set; }

        public DateTime? Completed { get; set; }
        
        [JsonIgnore]
        public TaskList ParentTaskList { get; set; } // Should not set both
        
        [JsonIgnore]
        public TaskModel Parent { get; set; }
        
        [JsonIgnore]
        public System.Collections.Generic.List<TaskModel> Children { get; set; }
    }
}