using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace ChoreBoard.Data.Models
{
    [Index(nameof(Uuid), IsUnique = true)]
    public class TaskDefinition
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string ShortDescription { get; set; }
        public string? Description { get; set; }

        public List<TaskInstance> TaskInstances { get; } = new();
        public List<TaskSchedule> Schedules { get; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}