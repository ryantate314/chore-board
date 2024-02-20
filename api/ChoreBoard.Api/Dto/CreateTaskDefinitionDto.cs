using ChoreBoard.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChoreBoard.Api.Dto
{
    public class CreateTaskDefinitionDto
    {
        [Required]
        public string ShortDescription { get; set; }
        public string? Description { get; set; }
        public int? Points { get; set; }

        public TaskCategory Category { get; set; } = TaskCategory.Family;

        [Required]
        public DateTime StartDate { get; set; }

        public bool DoesRepeat { get; set; } = false;

        public DateTime? EndDate { get; set; }
        public List<int> DaysOfWeek { get; set; } = new();
        public int? Count { get; set; }
        public Frequency Frequency { get; set; } = Frequency.None;
        public int? Interval { get; set; }
    }
}
