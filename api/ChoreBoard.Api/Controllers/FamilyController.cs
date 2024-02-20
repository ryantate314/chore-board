using ChoreBoard.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChoreBoard.Api.Controllers
{
    [ApiController]
    [Route("familyMembers")]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService _service;

        public FamilyController(IFamilyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var familyId = new Guid("A91A2410-77D6-405C-8920-80D3FD15367E");

            IEnumerable<Service.Models.FamilyMember> familyMembers = await _service.GetMembers(familyId);

            return Ok(familyMembers);
        }
    }
}
