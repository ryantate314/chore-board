using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace ChoreBoard.Service
{
    public interface ITaskService
    {
        Task<TaskInstance> CreateTask(TaskInstance taskInstance);
        Task<List<TaskInstance>> GetTasks(DateTime startDate, DateTime endDate);
        Task<TaskInstance> UpdateStatus(Guid id, string newStatus, Guid? familyMember);
    }
    public class TaskService : ITaskService
    {
        private readonly ITaskDefinitionRepo _taskDefinitionRepo;
        private readonly ITaskRepository _taskInstanceRepo;

        public TaskService(ITaskDefinitionRepo taskDefinitionRepo, ITaskRepository taskInstanceRepo)
        {
            _taskDefinitionRepo = taskDefinitionRepo;
            _taskInstanceRepo = taskInstanceRepo;
        }

        public Task<TaskInstance> CreateTask(TaskInstance taskInstance)
        {
            return _taskInstanceRepo.Create(taskInstance);
        }

        public async Task<List<TaskInstance>> GetTasks(DateTime startDate, DateTime endDate)
        {
            /*
             * const schedules = await taskRepository.getDefinitionSchedules(startDate, endDate);
         console.log("Found schedules: " + schedules);

         let allConcreteInstances = await taskRepository.getTasks(startDate, endDate);

         let allTasks: Task[] = [];

         for (let schedule of schedules) {
             // Construct and modify the options first, because the RRule object is immutable
             // https://github.com/jkbrzt/rrule#rruleparsestringrfcstring
             let options = RRule.parseString(schedule.rrule);
             options.dtstart = schedule.activeStartDate;
             options.until = schedule.activeEndDate;
             let rule = new RRule(options);

             let nextInstanceDate: Date | null = schedule.activeStartDate;

             let lastIncompleteTask = await taskRepository.getLastIncompleteTask(schedule.taskDefinitionId!);
             if (lastIncompleteTask == null) {
                 let lastCompletedTask = await taskRepository.getLastCompletedTask(schedule.taskDefinitionId!);
                 if (lastCompletedTask !== null) {
                     console.log("Last completed task", lastCompletedTask.instanceDate);
                     nextInstanceDate = rule.after(lastCompletedTask.instanceDate)
                 }
             }

             let allConcreteInstances = allConcreteInstances.filter(x =>
                 x.definition.id == schedule.taskDefinitionId
                 && (lastIncompleteTask == null || x.id != lastIncompleteTask.id)
             );

             let allInstances = allConcreteInstances;
             // If there is a pending incomplete task, don't add new tasks to the board
             if (lastIncompleteTask !== null)
                 allInstances = [
                     ...allInstances,
                     lastIncompleteTask
                 ];
             // If the next virtual instance is within the requested window, return a virtual task instance
             else if (nextInstanceDate != null && nextInstanceDate < endDate)
                 allInstances = [
                     ...allConcreteInstances,
                     {
                         id: null,
                         // TODO add error checking
                         definition: (await taskRepository.getDefinition(schedule.taskDefinitionId!))!,
                         instanceDate: nextInstanceDate,
                         // If the instance date is in the past
                         status: getStatus(null, nextInstanceDate),
                         createdAt: null
                     }
                 ];

             allInstances = allInstances.map(x => ({
                 ...x,
                 status: getStatus(x.status, x.instanceDate)
             }));

             allTasks = [
                 ...allTasks,
                 ...allInstances
             ];
         }

         return allTasks;
             */

            IEnumerable<TaskSchedule> schedules = await _taskDefinitionRepo.GetTaskSchedules(startDate, endDate);
            IEnumerable<TaskInstance> allConcreteInstances = await _taskInstanceRepo.GetTasks(startDate, endDate);

            IEnumerable<Guid> allTaskDefinitionIds = schedules.Select(x => x.TaskDefinitionId)
                .Concat(allConcreteInstances.Select(x => x.Definition.Id))
                .Distinct();

            Dictionary<Guid, TaskInstance> mostRecentTasks = await _taskInstanceRepo.GetMostRecentTask(allTaskDefinitionIds, endDate);
            IEnumerable<TaskDefinition> taskDefinitions = await _taskDefinitionRepo.GetTaskDefinitions(allTaskDefinitionIds);

            // Start with all non-scheduled tasks.
            List<TaskInstance> allTasks = allConcreteInstances.Where(x => !schedules.Any(y => y.TaskDefinitionId == x.Definition.Id))
                .ToList();

            foreach (TaskSchedule schedule in schedules)
            {
                DateTime? nextInstanceDate = schedule.StartDate;

                List<TaskInstance> instances = allConcreteInstances.Where(x => x.Definition.Id == schedule.TaskDefinitionId)
                    .ToList();

                var evnt = new CalendarEvent()
                {
                    Start = new CalDateTime(schedule.StartDate),
                    RecurrenceRules = new List<RecurrencePattern>()
                    {
                        schedule.Pattern
                    }
                };

                // If there is an open incomplete task, use that as the concrete instance even if it's outside the date window
                // If there is a completed task, use it as the start date for calculating the next instance
                TaskInstance? taskInstance = null;
                if (mostRecentTasks.TryGetValue(schedule.TaskDefinitionId, out taskInstance))
                {
                    if (taskInstance.Status == Models.TaskStatus.STATUS_COMPLETED
                        || taskInstance.Status == Models.TaskStatus.STATUS_DELETED)
                    {
                        // Task is complete
                        nextInstanceDate = evnt.GetOccurrences(taskInstance.InstanceDate, schedule.EndDate)
                            .FirstOrDefault()
                            ?.Period
                            .StartTime
                            .Value;
                    }
                    else
                    {
                        // Task is still Open
                        nextInstanceDate = null;

                        // If the open task is not inside the search window, show it anyway.
                        if (!instances.Any(x => x.Id == taskInstance.Id))
                            instances.Add(taskInstance);
                        else
                            taskInstance = instances.First(x => x.Id == taskInstance.Id);

                        // Choose a status if it's null.
                        taskInstance.Status = GetStatus(taskInstance.Status, taskInstance.InstanceDate);
                    }
                }

                if (nextInstanceDate != null)
                {
                    TaskDefinition definition = taskDefinitions.First(x => x.Id == schedule.TaskDefinitionId);

                    instances.Add(new TaskInstance()
                    {
                        Id = null,
                        InstanceDate = nextInstanceDate.Value,
                        Status = GetStatus(null, nextInstanceDate.Value),
                        Definition = definition
                    });
                }

                allTasks.AddRange(instances);
            }

            return allTasks.OrderBy(x => x.InstanceDate)
                .ToList();
        }

        public Task<TaskInstance> UpdateStatus(Guid id, string newStatus, Guid? familyMember)
        {
            return _taskInstanceRepo.UpdateStatus(id, newStatus, familyMember);
        }

        private string GetStatus(string? currentStatus, DateTime instanceDate)
        {
            if (currentStatus == null)
                return instanceDate > DateTime.Now
                    ? Models.TaskStatus.STATUS_UPCOMING
                    : Models.TaskStatus.STATUS_TODO;
            return currentStatus;
        }
    }
}