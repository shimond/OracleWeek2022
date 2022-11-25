using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.ObjectPool;
using System.Security.Claims;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        private readonly IProductsRepository _productsRepository;

        public ProductsController(ILogger<ProductsController> logger, 
            IProductsRepository productsRepository)
        {
            _logger = logger;
            _productsRepository = productsRepository;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        [ProducesResponseType(400, Type = typeof(string[]))]
        [OutputCache]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(10000);
            var res = await _productsRepository.GetAll();
            return Ok(res);
        }

        //:range(10,900)
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetById(int id)
        {
            var res = await _productsRepository.GetById(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewItem(Product p)
        {
            var res = await _productsRepository.AddNewProduct(p);
            if (res == null)
            {
                return NotFound();
            }
            return Created($"/api/products/{res.Id}", res);
        }

    }
}