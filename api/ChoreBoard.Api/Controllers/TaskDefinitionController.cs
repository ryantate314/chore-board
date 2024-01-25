using ChoreBoard.Api.Dto;
using ChoreBoard.Service;
using ChoreBoard.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoreBoard.Api.Controllers
{
    [Route("task-definitions")]
    [ApiController]
    public class TaskDefinitionController : ControllerBase
    {
        private readonly ITaskDefinitionService _service;
        private readonly ILogger<TaskDefinitionController> _logger;

        public TaskDefinitionController(ITaskDefinitionService service, ILogger<TaskDefinitionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<TaskDefinition> definitions = await _service.GetAll();

            return Ok(definitions);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDefinitionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogDebug("Creating new task definition: " + dto.ShortDescription);

            var definition = new TaskDefinition()
            {
                Description = dto.Description,
                ShortDescription = dto.ShortDescription,
            };

            if (dto.Frequency != Frequency.None)
            {
                var builder = new ScheduleBuilder(dto.StartDate);
                builder.SetFrequency(dto.Frequency);

                if (dto.EndDate != null)
                    builder.SetEndDate(dto.EndDate.Value);
                if (dto.Count > 0)
                    builder.SetCount(dto.Count.Value);
                if (dto.Interval > 1)
                    builder.SetInterval(dto.Interval.Value);
                if (dto.DaysOfWeek.Any())
                    builder.SetDaysOfWeek(dto.DaysOfWeek);

                definition.Schedules = new List<TaskSchedule>()
                {
                    builder.Build()
                };
            }

            TaskDefinition newDefinition = await _service.Create(definition);

            return Ok(newDefinition);
        }
    }
}
