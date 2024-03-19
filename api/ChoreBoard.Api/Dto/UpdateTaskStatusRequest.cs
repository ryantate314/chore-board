using ChoreBoard.Api.Validation;
using System;

namespace ChoreBoard.Api.Dto
{
    public class UpdateTaskStatusRequest
    {
        [TaskStatus]
        public string NewStatus { get; set; }
        public Guid? FamilyMemberId { get; set; }
    }
}
