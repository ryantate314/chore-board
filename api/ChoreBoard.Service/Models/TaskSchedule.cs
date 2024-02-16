using Ical.Net;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskSchedule
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RecurrencePattern Pattern { get; set; }

        public Guid TaskDefinitionId { get; set; }
    }
}
