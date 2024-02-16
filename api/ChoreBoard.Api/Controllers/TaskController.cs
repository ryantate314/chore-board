using ChoreBoard.Api.Dto;
using ChoreBoard.Service;
using ChoreBoard.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChoreBoard.Api.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        {
            List<TaskInstance> instances = await _service.GetTasks(startDate, endDate);

            return Ok(instances);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus([FromRoute] Guid id, [FromBody] UpdateTaskStatusRequest dto)
        {
            TaskInstance instance = await _service.UpdateStatus(id, dto.NewStatus, dto.FamilyMember);

            return Ok(instance);
        }
    }
}
