using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class FamilyMember
    {
        public FamilyMember()
        {
            Tasks = new HashSet<TaskInstance>();
        }

        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int FamilyId { get; set; }
        public virtual Family Family { get; set; } = null!;

        public virtual ICollection<TaskInstance> Tasks { get; set; }
    }
}
