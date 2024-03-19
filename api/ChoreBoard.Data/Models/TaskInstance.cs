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
        public string? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? CompletedById { get; set; }
        public int? Points { get; set; }

        public virtual TaskStatus? StatusNavigation { get; set; }
        public virtual TaskDefinition TaskDefinition { get; set; } = null!;
        public virtual FamilyMember? CompletedBy { get; set; } = null!;
    }
}
