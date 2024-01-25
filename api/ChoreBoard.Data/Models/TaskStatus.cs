using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class TaskStatus
    {
        public string StatusCode { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
