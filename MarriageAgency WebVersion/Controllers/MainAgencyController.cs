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
        public IActionResult GetUsers()
        {
            return Ok(_marriageAgnecyService.GetUsers());
        }

        [HttpGet]
        public IActionResult GetUserByName([FromQuery] string nameOfUser)
        {
            return Ok(_marriageAgnecyService.GetUserByName(nameOfUser));
        }
        
        [HttpPost]
        public IActionResult SendMessage([FromQuery] string messageContent, string messageReceiver)
        {
            return Ok(_marriageAgnecyService.SendMessage(messageContent, messageReceiver));
        }
    }
}