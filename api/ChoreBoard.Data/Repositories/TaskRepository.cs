using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChoreBoard.Data.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private readonly ChoreBoardContext _context;

        private readonly TaskInstanceMapper _mapper = new TaskInstanceMapper();

        public TaskRepository(ChoreBoardContext context)
        {
            _context = context;
        }

        private Task<TaskInstance> GetTask(Guid id)
        {
            return _context.TaskInstances.Where(x => x.Uuid == id)
                .Include(x => x.TaskDefinition)
                .Include(x => x.CompletedBy)
                .FirstAsync();
        }

        public async Task<IEnumerable<Service.Models.TaskInstance>> GetTasks(DateTime startDate, DateTime endDate)
        {
            List<Models.TaskInstance> instances = await _context.TaskInstances
                .Include(x => x.TaskDefinition)
                .Include(x => x.CompletedBy)
                .Where(x =>
                    ( x.InstanceDate >= startDate && x.InstanceDate < endDate )
                    || (x.CompletedAt >= startDate && x.CompletedAt < endDate )
                )
                .Where(x => x.TaskDefinition.DeletedAt == null)
                .ToListAsync();

            return instances.Select(_mapper.Map)
                .ToList();
        }

        public async Task<Dictionary<Guid, Service.Models.TaskInstance>> GetMostRecentTask(IEnumerable<Guid> taskDefinitionIds, DateTime before)
        {
            List<Models.TaskInstance> instances = await _context.TaskInstances
                .Include(x => x.TaskDefinition)
                .Include(x => x.CompletedBy)
                .Where(x =>
                    taskDefinitionIds.Contains(x.TaskDefinition.Uuid)
                    //&& x.InstanceDate < before
                    //&& (x.Status == null || x.Status != Service.Models.TaskStatus.STATUS_DELETED)
                )
                .GroupBy(x => x.TaskDefinition.Uuid)
                .Select(x =>
                    x.OrderByDescending(y => y.InstanceDate)
                        // Prioritize real instances over deleted ones, but still use deleted instances
                        // for date calculations
                        .ThenBy(y => y.Status != Service.Models.TaskStatus.STATUS_DELETED ? 0 : 10)
                        .First()
                )
                .ToListAsync();

            return instances.Select(_mapper.Map)
                .ToDictionary(x => x.Definition.Id);
        }

        public async Task<Service.Models.TaskInstance> Create(Service.Models.TaskInstance task)
        {
            TaskInstance taskModel = _mapper.Map(task);

            TaskDefinition definition = await _context.TaskDefinitions.SingleAsync(x => x.Uuid == taskModel.TaskDefinition.Uuid);
            taskModel.TaskDefinition = definition;

            _context.TaskInstances.Add(taskModel);

            await _context.SaveChangesAsync();

            return _mapper.Map(taskModel);
        }

        public async Task<Service.Models.TaskInstance> UpdateStatus(Guid taskId, string? status, Guid? familyMemberId)
        {
            TaskInstance task = await _context.TaskInstances
                .Include(x => x.TaskDefinition)
                .SingleAsync(x => x.Uuid == taskId);

            task.Status = status;

            if (status == Service.Models.TaskStatus.STATUS_COMPLETED)
            {
                task.CompletedAt = DateTime.UtcNow;

                if (familyMemberId != null)
                {
                    FamilyMember? familyMember = await _context.FamilyMembers.SingleAsync(x => x.Uuid == familyMemberId);
                    task.CompletedById = familyMember.Id;
                }
            }
            else
            {
                task.CompletedAt = null;
                task.CompletedById = null;
            }

            // TODO: log status change history

            await _context.SaveChangesAsync();

            return _mapper.Map(task);
        }
    }
}
