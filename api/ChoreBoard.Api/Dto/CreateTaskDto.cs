using ChoreBoard.Api.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChoreBoard.Api.Dto
{
    public class CreateTaskDto
    {
        [Required]
        public DateTime? InstanceDate { get; set; }
        
        [TaskStatus]
        public string? Status { get; set; }
    }
}
