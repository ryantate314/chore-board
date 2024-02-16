using ChoreBoard.Service;
using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBoard.UnitTests.Service
{
    public class TaskServiceTests
    {
        private ITaskDefinitionRepo _fakeTaskDefnitionRepo;
        private ITaskRepository _fakeTaskRepo;

        private TaskService _service;

        [SetUp]
        public void SetUp()
        {
            _fakeTaskDefnitionRepo = A.Fake<ITaskDefinitionRepo>();
            _fakeTaskRepo = A.Fake<ITaskRepository>();

            _service = new TaskService(
                _fakeTaskDefnitionRepo,
                _fakeTaskRepo
            );
        }

        [Test]
        public async Task GetTasks_DailyTaskWithNoPreviousInstances_UsesStartDate()
        {
            // Arrange
            DateTime seachStartDate = DateTime.Now.Date.AddDays(-3);
            DateTime searchEndDate = seachStartDate.AddDays(3);

            DateTime expectedInstanceDate = seachStartDate.AddDays(1)
                .AddHours(13)
                .AddMinutes(30);

            Guid taskDefinitionId = Guid.NewGuid();
           

            var schedules = new List<TaskSchedule>()
            {
                new TaskSchedule()
                {
                    StartDate = expectedInstanceDate,
                    EndDate = expectedInstanceDate.AddDays(100),
                    Pattern = new Ical.Net.DataTypes.RecurrencePattern()
                    {
                        Frequency = Ical.Net.FrequencyType.Daily
                    },
                    TaskDefinitionId = taskDefinitionId
                }
            };
            A.CallTo(() => _fakeTaskDefnitionRepo.GetTaskSchedules(A<DateTime>._, A<DateTime>._))
                .Returns(schedules);

            // No tasks
            A.CallTo(() => _fakeTaskRepo.GetMostRecentTask(A<IEnumerable<Guid>>._, A<DateTime>._));

            var taskDefinition = new TaskDefinition()
            {
                Id = taskDefinitionId
            };
            A.CallTo(() => _fakeTaskDefnitionRepo.GetTaskDefinitions(A<IEnumerable<Guid>>.Ignored))
                .Returns(new List<TaskDefinition>()
                {
                    { taskDefinition }
                });

            // Act
            List<TaskInstance> result = await _service.GetTasks(seachStartDate, searchEndDate);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            
            var instance = result[0];
            Assert.That(instance.Id, Is.Null);
            Assert.That(instance.InstanceDate, Is.EqualTo(expectedInstanceDate));
            Assert.That(instance.Definition, Is.Not.Null);
            Assert.That(instance.Definition.Id, Is.EqualTo(taskDefinitionId));
        }

        [Test]
        public async Task GetTasks_IncompleteInstanceInsideSearchWindow_UsesIncompleteInstance()
        {
            // Arrange
            DateTime seachStartDate = DateTime.Now.Date.AddDays(-3);
            DateTime searchEndDate = seachStartDate.AddDays(3);

            DateTime expectedInstanceDate = seachStartDate.AddDays(1)
                .AddHours(13)
                .AddMinutes(30);

            Guid taskDefinitionId = Guid.NewGuid();

            var schedules = new List<TaskSchedule>()
            {
                new TaskSchedule()
                {
                    StartDate = expectedInstanceDate,
                    EndDate = expectedInstanceDate.AddDays(100),
                    Pattern = new Ical.Net.DataTypes.RecurrencePattern()
                    {
                        Frequency = Ical.Net.FrequencyType.Daily
                    },
                    TaskDefinitionId = taskDefinitionId
                }
            };
            A.CallTo(() => _fakeTaskDefnitionRepo.GetTaskSchedules(A<DateTime>._, A<DateTime>._))
                .Returns(schedules);

            var pendingInstance = new TaskInstance()
            {
                Id = Guid.NewGuid(),
                InstanceDate = expectedInstanceDate,
                Definition = new TaskDefinition()
                {
                    Id = taskDefinitionId
                }
            };
            A.CallTo(() => _fakeTaskRepo.GetMostRecentTask(A<IEnumerable<Guid>>._, A<DateTime>._))
                .Returns(new Dictionary<Guid, TaskInstance>()
                {
                    { pendingInstance.Definition.Id, pendingInstance }
                });

            A.CallTo(() => _fakeTaskRepo.GetTasks(seachStartDate, searchEndDate))
                .Returns(new List<TaskInstance>()
                {
                    pendingInstance
                });

            // Act
            List<TaskInstance> result = await _service.GetTasks(seachStartDate, searchEndDate);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var instance = result[0];
            Assert.That(instance.Id, Is.EqualTo(pendingInstance.Id));
            Assert.That(instance.InstanceDate, Is.EqualTo(expectedInstanceDate));
        }

        [Test]
        public async Task GetTasks_IncompleteInstanceOutsideSearchWindow_UsesIncompleteInstance()
        {
            // Arrange
            DateTime seachStartDate = DateTime.Now.Date.AddDays(-3);
            DateTime searchEndDate = seachStartDate.AddDays(3);

            DateTime expectedInstanceDate = seachStartDate.AddDays(-1)
                .AddHours(13)
                .AddMinutes(30);

            Guid taskDefinitionId = Guid.NewGuid();

            var schedules = new List<TaskSchedule>()
            {
                new TaskSchedule()
                {
                    StartDate = expectedInstanceDate,
                    EndDate = expectedInstanceDate.AddDays(100),
                    Pattern = new Ical.Net.DataTypes.RecurrencePattern()
                    {
                        Frequency = Ical.Net.FrequencyType.Daily
                    },
                    TaskDefinitionId = taskDefinitionId
                }
            };
            A.CallTo(() => _fakeTaskDefnitionRepo.GetTaskSchedules(A<DateTime>._, A<DateTime>._))
                .Returns(schedules);

            var pendingInstance = new TaskInstance()
            {
                Id = Guid.NewGuid(),
                InstanceDate = expectedInstanceDate,
                Definition = new TaskDefinition()
                {
                    Id = taskDefinitionId
                }
            };
            A.CallTo(() => _fakeTaskRepo.GetMostRecentTask(A<IEnumerable<Guid>>._, A<DateTime>._))
                .Returns(new Dictionary<Guid, TaskInstance>()
                {
                    { pendingInstance.Definition.Id, pendingInstance }
                });

            A.CallTo(() => _fakeTaskRepo.GetTasks(seachStartDate, searchEndDate))
                .Returns(new List<TaskInstance>() { });

            // Act
            List<TaskInstance> result = await _service.GetTasks(seachStartDate, searchEndDate);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var instance = result[0];
            Assert.That(instance.Id, Is.EqualTo(pendingInstance.Id));
            Assert.That(instance.InstanceDate, Is.EqualTo(expectedInstanceDate));
        }
    }
}
