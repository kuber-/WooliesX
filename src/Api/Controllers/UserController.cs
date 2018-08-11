using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/answers/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        static readonly string TokenKey = "Api:Token";
        readonly IConfiguration configuration;

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Name = "Kuber",
                Token = configuration[TokenKey],
            });
        }

        [HttpOptions]
        public IActionResult UserOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }
    }
}
