using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductCommand _productCommand;
        private readonly ProductQuery _productQuery;

        public ProductsController()
        {
            _productCommand = new ProductCommand();
            _productQuery = new ProductQuery();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateProduct([FromForm] ProductModel product)
        {
            try
            {
                var result = await _productCommand.CreateProduct(product, Request);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creando producto.");
            }
        }

        [HttpGet("GetAllProducts")]
        public ActionResult<List<ProductModel>> GetAllProducts()
        {
            try
            {
                var products = _productQuery.GetAllProducts();
                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo productos.");
            }
        }
    }
}
