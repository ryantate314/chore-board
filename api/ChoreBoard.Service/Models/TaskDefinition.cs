using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskDefinition
    {
        public Guid Id { get; set; }
        public string ShortDescription { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Points { get; set; }
        public TaskCategory Category { get; set; }

        [JsonIgnore]
        public List<TaskSchedule>? Schedules {get; set; }
    }
}
