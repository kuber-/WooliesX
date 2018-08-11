using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/answers/trolleyCalculator")]
    [Route("api/answers/trolleyTotal")]
    [ApiController]
    public class TrolleyCalculator : ControllerBase
    {
        private readonly IProductsService productsService;

        public TrolleyCalculator(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public IActionResult Calculate(
        [FromBody] TrolleyForCalculateDto dto)
        {
            var getTotal = productsService.GetTotal(dto);
            return Ok(getTotal);
        }

        [HttpOptions]
        public IActionResult TrolleyCalculatorOptions()
        {
            Response.Headers.Add("Allow", "POST,OPTIONS");
            return Ok();
        }
    }
}
