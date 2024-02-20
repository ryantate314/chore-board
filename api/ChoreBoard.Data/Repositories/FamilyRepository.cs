using ChoreBoard.Data.Mapping;
using ChoreBoard.Data.Models;
using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Repositories
{
    internal class FamilyRepository : IFamilyMemberRepository
    {
        private readonly ChoreBoardContext _context;
        private readonly ILogger<TaskDefinitionRepository> _logger;
        private readonly FamilyMemberMapper _mapper;

        public FamilyRepository(ChoreBoardContext context, ILogger<TaskDefinitionRepository> logger)
        {
            _context = context;
            _logger = logger;

            _mapper = new FamilyMemberMapper();
        }

        public async Task<IEnumerable<Service.Models.FamilyMember>> GetMembers(Guid familyId)
        {
            List<Data.Models.FamilyMember> members = await _context.FamilyMembers.Where(x => x.Family.Uuid == familyId)
                .ToListAsync();

            return members.Select(_mapper.Map)
                .ToList();
        }
    }
}
