using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskStatus
    {
        public const string STATUS_DELETED = "D";
        public const string STATUS_COMPLETED = "C";
        public const string STATUS_UPCOMING = "U";
        public const string STATUS_TODO = "T";
        public const string STATUS_IN_PROGRESS = "I";

        public static readonly IReadOnlySet<string> AllStatuses = new HashSet<string>()
        {
            STATUS_DELETED,
            STATUS_COMPLETED,
            STATUS_UPCOMING,
            STATUS_TODO,
            STATUS_IN_PROGRESS
        };
    }
}
