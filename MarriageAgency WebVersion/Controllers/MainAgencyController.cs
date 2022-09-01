using MarriageAgency.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarriageAgency_WebVersion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainAgencyController : Controller
    {
        public IMarriageAgencyService _marriageAgnecyService;

        public MainAgencyController(IMarriageAgencyService marriageAgencyService)
        {
            _marriageAgnecyService = marriageAgencyService;
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_marriageAgnecyService.GetUsers());
        }

        [HttpGet]
        [Route("GetBestCandidates")]
        public IActionResult GetBestCandidates()
        {
            return Ok(_marriageAgnecyService.GetUsers());
        }

        [HttpGet]
        [Route("GetUserByName")]
        public IActionResult GetUserByName([FromQuery] string nameOfUser)
        {
            return Ok(_marriageAgnecyService.GetUserByName(nameOfUser));
        }
        
        [HttpPost]
        [Route("SendInvitation")]
        public IActionResult SendMessage([FromQuery] string messageContent, string messageReceiver)
        {
            return Ok(_marriageAgnecyService.SendMessage(messageContent, messageReceiver));
        }
    }
}