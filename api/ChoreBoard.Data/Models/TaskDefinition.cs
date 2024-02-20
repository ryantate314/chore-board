using ChoreBoard.Service.Models;
using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class TaskDefinition
    {
        public TaskDefinition()
        {
            TaskInstances = new HashSet<TaskInstance>();
            TaskSchedules = new HashSet<TaskSchedule>();
        }

        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string ShortDescription { get; set; } = null!;
        public string? Description { get; set; }
        public int? Points { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<TaskInstance> TaskInstances { get; set; }
        public virtual ICollection<TaskSchedule> TaskSchedules { get; set; }
    }
}
