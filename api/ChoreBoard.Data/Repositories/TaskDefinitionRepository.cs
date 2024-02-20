﻿using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChoreBoard.Api.Common;

namespace ChoreBoard.Data.Repositories
{
    internal class TaskDefinitionRepository : ITaskDefinitionRepo
    {
        private readonly ChoreBoardContext _context;
        private readonly ILogger<TaskDefinitionRepository> _logger;

        private readonly TaskDefinitionMapper _mapper = new TaskDefinitionMapper();
        private readonly TaskScheduleMapper _scheduleMapper = new TaskScheduleMapper();

        public TaskDefinitionRepository(ChoreBoardContext context, ILogger<TaskDefinitionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Service.Models.TaskDefinition>> GetTaskDefinitions()
        {
            List<Models.TaskDefinition> definitions = await _context.TaskDefinitions
                //.Include(x => x.Schedules)
                .Where(x => x.DeletedAt == null)
                .ToListAsync();

            return definitions
                .Select(_mapper.Map)
                .ToList();
        }

        public async Task<IEnumerable<Service.Models.TaskDefinition>> GetTaskDefinitions(IEnumerable<Guid> ids)
        {
            List<Models.TaskDefinition> definitions = await _context.TaskDefinitions
                .Where(x => ids.Contains(x.Uuid))
                .ToListAsync();

            return definitions
                .Select(_mapper.Map)
                .ToList();
        }

        public async Task<Service.Models.TaskDefinition> CreateTaskDefinition(Service.Models.TaskDefinition definition)
        {
            Data.Models.TaskDefinition model = _mapper.Map(definition);

            _logger.LogDebugJson("Creating new TaskDefinition", model);

            _context.TaskDefinitions.Add(model);

            await _context.SaveChangesAsync();

            return _mapper.Map(model);
        }

        public async Task<IEnumerable<Service.Models.TaskSchedule>> GetTaskSchedules(DateTime startDate, DateTime endDate)
        {
            List<Models.TaskSchedule> schedules = await _context.TaskSchedules
                .Include(x => x.TaskDefinition)
                // Checking between dates: https://stackoverflow.com/a/325964
                .Where(x => x.StartDate <= endDate && x.EndDate >= startDate)
                .Where(x => x.TaskDefinition.DeletedAt == null)
                .ToListAsync();

            return schedules.Select(_scheduleMapper.Map)
                .ToList();
        }

        public async Task<IEnumerable<Service.Models.TaskDefinition>> GetTaskDefinitionsByName(string shortDescription)
        {
            List<TaskDefinition> definitions = await _context.TaskDefinitions
                .Include(x => x.TaskSchedules)
                .Where(x => x.ShortDescription == shortDescription && x.DeletedAt == null)
                .ToListAsync();

            return definitions.Select(_mapper.Map)
                .ToList();
        }
    }
}
