using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreBoard.Api.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChoreBoard.Data.Repositories
{
    public class TaskDefinitionRepository : ITaskDefinitionRepo
    {
        private readonly ChoreBoardContext _context;
        private readonly ILogger<TaskDefinitionRepository> _logger;
        private readonly TaskDefinitionMapper _mapper = new TaskDefinitionMapper();

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
                .Select(_mapper.Map);
        }

        public async Task<Service.Models.TaskDefinition> CreateTaskDefinition(Service.Models.TaskDefinition definition)
        {
            Data.Models.TaskDefinition model = _mapper.Map(definition);

            _logger.LogDebugJson("Creating new TaskDefinition", model);

            EntityEntry<Models.TaskDefinition> entity = _context.TaskDefinitions.Add(model);

            await _context.SaveChangesAsync();

            return _mapper.Map(model);
        }
    }
}
