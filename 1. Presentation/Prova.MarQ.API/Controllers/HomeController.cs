using Microsoft.AspNetCore.Mvc;

namespace Prova.MarQ.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok("Boa prova! :)");
        }
    }
}
