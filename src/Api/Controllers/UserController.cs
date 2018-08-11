using Microsoft.AspNetCore.Mvc;
using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [Route("api/answers/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string token;

        public UserController(IOptions<ApiOptions> apiOptions)
        {
            token = apiOptions.Value.Token;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Name = "Kuber Kumar",
                Token = token,
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
