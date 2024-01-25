using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class TaskInstance
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int TaskDefinitionId { get; set; }
        public DateTime InstanceDate { get; set; }

        public virtual TaskDefinition TaskDefinition { get; set; } = null!;
    }
}
