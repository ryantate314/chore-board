using ChoreBoard.Service.Models;
using System.ComponentModel.DataAnnotations;

namespace ChoreBoard.Api.Validation
{
    public class TaskStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            string? taskStatus = (string?)value;

            if (taskStatus != null && !TaskStatus.AllStatuses.Contains(taskStatus))
            {
                return new ValidationResult("Invalid task status.");
            }

            return ValidationResult.Success;
        }
    }
}
