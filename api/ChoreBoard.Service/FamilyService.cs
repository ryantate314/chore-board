using ChoreBoard.Service.Models;
using ChoreBoard.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service
{
    public interface IFamilyService
    {
        Task<IEnumerable<FamilyMember>> GetMembers(Guid familyId);
    }

    public class FamilyService : IFamilyService
    {
        private readonly IFamilyMemberRepository _repo;

        public FamilyService(IFamilyMemberRepository repo)
        {
            _repo = repo;
        }
        public Task<IEnumerable<FamilyMember>> GetMembers(Guid familyId)
        {
            return _repo.GetMembers(familyId);
        }
    }
}
