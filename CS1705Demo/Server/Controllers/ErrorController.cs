using Microsoft.AspNetCore.Mvc;

namespace ServerSide.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
