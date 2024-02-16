using ChoreBoard.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskInstance> Create(TaskInstance task);
        Task<IEnumerable<TaskInstance>> GetTasks(DateTime startDate, DateTime endDate);
        Task<Dictionary<Guid, TaskInstance>> GetMostRecentTask(IEnumerable<Guid> taskDefinitionIds, DateTime before);
        Task<TaskInstance> UpdateStatus(Guid taskId, string? status, Guid? familyMember);
    }
}
