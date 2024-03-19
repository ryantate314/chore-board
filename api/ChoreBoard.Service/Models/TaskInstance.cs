using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskInstance
    {
        public Guid? Id { get; set; }
        public DateTime InstanceDate { get; set; }
        public TaskDefinition Definition { get; set; }
        public string? Status { get;set; }
        public DateTime? CompletedAt { get; set; }
        public int? Points { get; set; }

        public FamilyMember? CompletedBy { get; set; }
    }
}
