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
        [Route("GetCachedUsers")]
        public IActionResult GetCachedUsers()
        {
            return Ok(_marriageAgencyService.GetCachedUsers());
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

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] int idOfUser)
        {
            var res = await _marriageAgencyService.GetUserById(idOfUser);

            return Ok(res);
        }

        [HttpPost]
        [Route("SendInvitation")]
        public IActionResult SendInvitation([FromQuery] string messageContent, string messageReceiver)
        {
            return Ok(_marriageAgencyService.SendInvitation(messageContent, messageReceiver));
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromBody] User newUser)
        {
            return Ok(_marriageAgencyService.AddUser(newUser));
        }

        [HttpPost]
        [Route("AddRequirement")]
        public IActionResult AddRequirements([FromBody] Requirement newRequirement)
        {
            return Ok(_marriageAgencyService.AddRequirement(newRequirement));
        }
    }
}