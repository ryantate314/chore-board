using ChoreBoard.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Repositories
{
    public interface ITaskDefinitionRepo
    {
        Task<TaskDefinition> CreateTaskDefinition(TaskDefinition definition);
        public Task<IEnumerable<TaskDefinition>> GetTaskDefinitions();
        public Task<IEnumerable<TaskDefinition>> GetTaskDefinitions(IEnumerable<Guid> ids);
        Task<IEnumerable<TaskDefinition>> GetTaskDefinitionsByName(string shortDescription);
        Task<IEnumerable<TaskSchedule>> GetTaskSchedules(DateTime startDate, DateTime endDate);
    }
}
