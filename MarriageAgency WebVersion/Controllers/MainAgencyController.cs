using MarriageAgency.BLL.Services;
using MarriageAgency.Shared.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetUsers()
        {
            return Ok(_marriageAgencyService.GetUsers());
        }

        [HttpGet]
        [Route("GetBestCandidates")]
        public IActionResult GetBestCandidates([FromQuery] string clientId)
        {
            return Ok(_marriageAgencyService.GetBestCandidates(clientId));
        }

        [HttpGet]
        [Route("GetUserByName")]
        public IActionResult GetUserByName([FromQuery] string nameOfUser)
        {
            return Ok(_marriageAgencyService.GetUserByName(nameOfUser));
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
    }
}