using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class TaskStatus
    {
        public TaskStatus()
        {
            TaskInstances = new HashSet<TaskInstance>();
        }

        public string StatusCode { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<TaskInstance> TaskInstances { get; set; }
    }
}
