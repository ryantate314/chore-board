using ChoreBoard.Data.Mapping;

namespace CoreBoard.UnitTests.Data
{
    public class TaskDefinitionMapperTests
    {
        private TaskDefinitionMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new TaskDefinitionMapper();
        }

        [Test]
        public void TestMap_ServiceToData_WithSchedule()
        {
            Guid taskDefinitionId = Guid.NewGuid();
            string rrule = "FREQ=DAILY";
            // Arrange
            var serviceModel = new ChoreBoard.Service.Models.TaskDefinition()
            {
                Description = "Test",
                Id = taskDefinitionId,
                ShortDescription = "Short Description",
                Schedules = new List<ChoreBoard.Service.Models.TaskSchedule>()
                {
                    new ChoreBoard.Service.Models.TaskSchedule()
                    {
                        EndDate = DateTime.Now.AddDays(10),
                        Pattern = new Ical.Net.DataTypes.RecurrencePattern(rrule),
                        StartDate = DateTime.Now,
                        TaskDefinitionId = taskDefinitionId
                    }
                }
            };

            // Act
            var dataModel = _mapper.Map(serviceModel);

            // Assert
            Assert.That(dataModel.TaskSchedules, Is.Not.Empty);
            Assert.That(dataModel.TaskSchedules.First().RRule, Is.EqualTo(rrule));
            Assert.That(dataModel.TaskSchedules.First().TaskDefinition?.Uuid, Is.EqualTo(taskDefinitionId));
        }
    }
}