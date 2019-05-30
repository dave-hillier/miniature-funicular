using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tasks.Model
{
    public class TaskModel
    {
        [JsonIgnore]
        public string Id { get; set; }

        [MaxLength(64)]
        [JsonIgnore]
        [Required]
        public string Tenant { get; set; } // Consider removing this and only honouring the parent task list

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        [Required]
        [MaxLength(20)]
        public string Position { get; set; }

        public string Notes { get; set; }

        [Required]
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