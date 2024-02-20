using ChoreBoard.Service.Exceptions;
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

        public async Task<TaskDefinition> Create(TaskDefinition taskDefinition)
        {
            IEnumerable<TaskDefinition> existingDefinitiosn = await _taskDefinitionRepo.GetTaskDefinitionsByName(taskDefinition.ShortDescription);

            if (existingDefinitiosn.Any())
                throw new AlreadyExistsException($"A task definition already exists with name '{taskDefinition.ShortDescription}'.");

            return await _taskDefinitionRepo.CreateTaskDefinition(taskDefinition);
        }
    }
}
