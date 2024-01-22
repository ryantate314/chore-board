using ChoreBoard.Api.Dto;
using ChoreBoard.Service;
using ChoreBoard.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChoreBoard.Api.Controllers
{
    [Route("task-definitions")]
    [ApiController]
    public class TaskDefinitionController : ControllerBase
    {
        private readonly ITaskDefinitionService _service;

        public TaskDefinitionController(ITaskDefinitionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var definitions = await _service.GetAll();

            return Ok(definitions);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDefinitionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
