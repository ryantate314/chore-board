using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class TaskSchedule
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RRule { get; set; } = null!;
        public int TaskDefinitionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual TaskDefinition TaskDefinition { get; set; } = null!;
    }
}
