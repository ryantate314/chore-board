using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Models
{
    public class TaskSchedule
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        // Need this to optimize db queries
        public DateTime EndDate { get; set; }
        public string RRule { get; set; }

        public int TaskDefinitionId { get; set; }
        public TaskDefinition TaskDefinition { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
