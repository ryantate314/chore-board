using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Repositories
{
    public class TaskDefinitionRepository : ITaskDefinitionRepo
    {
        private readonly TaskContext _context;
        private readonly TaskDefinitionMapper _mapper = new TaskDefinitionMapper();

        public TaskDefinitionRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service.Models.TaskDefinition>> GetTaskDefinitions()
        {
            List<Models.TaskDefinition> definitions = await _context.TaskDefinitions
                .Include(x => x.Schedules)
                .Where(x => x.DeletedAt == null)
                .ToListAsync();

            return definitions
                .Select(_mapper.Map);
        }

        public async Task<Service.Models.TaskDefinition> CreateTaskDefinition(Service.Models.TaskDefinition definition)
        {
            Data.Models.TaskDefinition model = _mapper.Map(definition);

            _context.TaskDefinitions.Add(model);
            await _context.SaveChangesAsync();

            return _mapper.Map(model);
        }
    }
}
