using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Models
{
    public class Family
    {
        public Family()
        {
            FamilyMembers = new HashSet<FamilyMember>();
        }

        public int Id { get; set; }
        public Guid Uuid { get; internal set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}
