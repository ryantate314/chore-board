using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service
{
    public interface ITaskDefinitionService
    {
        Task<TaskDefinition> Create(TaskDefinition taskDefinition);
        Task<IEnumerable<TaskDefinition>> GetAll();
    }

    public class TaskDefinitionService : ITaskDefinitionService
    {
        private readonly ITaskDefinitionRepo _taskDefinitionRepo;

        public TaskDefinitionService(ITaskDefinitionRepo taskDefinitionRepo)
        {
            _taskDefinitionRepo = taskDefinitionRepo;
        }

        public Task<IEnumerable<TaskDefinition>> GetAll()
        {
            return _taskDefinitionRepo.GetTaskDefinitions();
        }

        public Task<TaskDefinition> Create(TaskDefinition taskDefinition)
        {
            return _taskDefinitionRepo.CreateTaskDefinition(taskDefinition);
        }
    }
}
