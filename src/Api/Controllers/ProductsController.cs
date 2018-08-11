using Microsoft.AspNetCore.Mvc;
using Api.Options;
using Api.Helpers;
using Api.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Api.Controllers
{
    [Route("api/answers/sort")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ProductsResourceParameters parameters)
        {
            var products = await productsService.GetSortedProductsAsync(parameters.SortOption.Value);
            return Ok(products);
        }

        [HttpOptions]
        public IActionResult ProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }
    }
}
