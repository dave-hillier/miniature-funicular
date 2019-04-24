using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Model
{
    public class TaskList
    {
        public string Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
                
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        
        // Not on end user model - usually group, sometimes site
        [Required]
        [MaxLength(20)]
        public string Tenant { get; set; }
        
        public List<TaskModel> Tasks { get; set; }
    }
}