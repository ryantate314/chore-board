using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskDefinition
    {
        public Guid Id { get; set; }
        public string ShortDescription { get; set; }
        public string? Description { get; set; }

        public List<TaskSchedule>? Schedules {get; set; }
    }
}
