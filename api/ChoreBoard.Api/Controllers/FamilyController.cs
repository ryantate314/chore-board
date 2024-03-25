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
            var familyId = new Guid("764876C8-AD73-457B-BF8C-40557B895A8D");

            IEnumerable<Service.Models.FamilyMember> familyMembers = await _service.GetMembers(familyId);

            return Ok(familyMembers);
        }
    }
}
