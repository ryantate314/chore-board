using AutoMapper;
using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBoard.UnitTests.Data
{
    internal class TaskInstanceMapperTests
    {
        private TaskInstanceMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new TaskInstanceMapper();
        }

        [Test]
        public void TestMap_DataToService_Incomplete_Succeeds()
        {
            // Arrange
            var instance = new TaskInstance()
            {
                Id = 1,
                Uuid = Guid.NewGuid(),
                CompletedBy = null
            };

            // Act
            var mappedTask = _mapper.Map(instance);

            // Assert
            Assert.That(mappedTask.Id, Is.EqualTo(instance.Uuid));

            Assert.That(mappedTask.CompletedBy, Is.Null);
        }

        [Test]
        public void TestMap_DataToService_Succeeds()
        {
            // Arrange
            var instance = new TaskInstance()
            {
                Id = 1,
                Uuid = Guid.NewGuid(),
                CompletedBy = new FamilyMember()
                {
                    Id = 1,
                    Uuid = Guid.NewGuid()
                }
            };

            // Act
            var mappedTask = _mapper.Map(instance);

            // Assert
            Assert.That(mappedTask.Id, Is.EqualTo(instance.Uuid));
            
            Assert.That(mappedTask.CompletedBy, Is.Not.Null);
            Assert.That(mappedTask.CompletedBy.Id, Is.EqualTo(instance.CompletedBy.Uuid));
        }
    }
}
