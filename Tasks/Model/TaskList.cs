using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Tasks.Model
{
    public class TaskList
    {
        [JsonIgnore]
        public string Id { get; set; }
        
        [MaxLength(255)]
        public string Title { get; set; }
                
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        
        [JsonIgnore]
        [MaxLength(64)]
        public string Tenant { get; set; }
        
        [JsonIgnore]
        public List<TaskModel> Tasks { get; set; }
    }
}