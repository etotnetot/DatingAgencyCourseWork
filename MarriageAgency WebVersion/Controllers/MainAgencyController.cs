using MarriageAgency.BLL.Services;
using MarriageAgency.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarriageAgency_WebVersion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainAgencyController : Controller
    {
        private readonly IMarriageAgencyService _marriageAgencyService;

        public MainAgencyController(IMarriageAgencyService marriageAgencyService)
        {
            _marriageAgencyService = marriageAgencyService;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _marriageAgencyService.GetUsers());
        }

        [HttpGet]
        [Route("GetBestCandidates")]
        public async Task<IActionResult> GetBestCandidates([FromQuery] string userLogin)
        {
            return Ok(await _marriageAgencyService.GetBestCandidates(userLogin));
        }

        [HttpGet]
        [Route("GetUserByName")]
        public async Task<IActionResult> GetUserByName([FromQuery] string nameOfUser)
        {
            var res = await _marriageAgencyService.GetUserByName(nameOfUser);

            return Ok(res);
        }
        
        [HttpPost]
        [Route("SendInvitation")]
        public IActionResult SendInvitation([FromQuery] string messageContent, string messageReceiver)
        {
            return Ok(_marriageAgencyService.SendInvitation(messageContent, messageReceiver));
        }

        [HttpPatch]
        [Route("UpdateRequirements")]
        public IActionResult UpdateRequirements([FromQuery] Requirement newRequirement)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser([FromQuery] int userId)
        {
            return Ok();
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromQuery] User newUser)
        {
            return Ok(_marriageAgencyService.AddUser(newUser));
        }

        [HttpPost]
        [Route("AddRequirement")]
        public IActionResult AddRequirements([FromQuery] Requirement newRequirement)
        {
            return Ok(_marriageAgencyService.AddRequirement(newRequirement));
        }
    }
}