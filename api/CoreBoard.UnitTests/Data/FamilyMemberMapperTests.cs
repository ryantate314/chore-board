using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBoard.UnitTests.Data
{
    public class FamilyMemberMapperTests
    {
        private FamilyMemberMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new FamilyMemberMapper();
        }

        [Test]
        public void TestMap_DataToService_Succeeds()
        {
            // Arrange
            var member = new FamilyMember()
            {
                CreatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
                Id = 1,
                Uuid = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
            };

            // Act
            ChoreBoard.Service.Models.FamilyMember mappedMember = _mapper.Map(member);

            // Assert
            Assert.That(mappedMember.Id, Is.EqualTo(member.Uuid));
        }
    }
}
