using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Model
{
    public class TaskModel
    {
        public string Id { get; set; }
                
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        
        // Not on end user model - usually group, sometimes site
        [Required]
        [MaxLength(20)]
        public string Tenant { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string Status { get; set; } // TODO: enum?

        public DateTime? Due { get; set; }

        public DateTime? Completed { get; set; }
        
        public TaskList ParentTaskList { get; set; } // Should not set both
        
        public TaskModel Parent { get; set; }
        
        public System.Collections.Generic.List<TaskModel> Children { get; set; }
    }
}