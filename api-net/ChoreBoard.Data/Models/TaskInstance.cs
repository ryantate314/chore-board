using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Models
{
    [Index(nameof(Uuid), IsUnique = true)]
    public class TaskInstance
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }

        public int? StatusId { get; set; }
        public TaskStatus? Status { get; set; }

        public int TaskDefinitionId { get; set; }
        public TaskDefinition TaskDefinition { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
