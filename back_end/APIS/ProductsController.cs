using back_end.Domain;
using back_end.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductHandler _productHandler;

        public ProductsController()
        {
            _productHandler = new ProductHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CrearProducto([FromForm] ProductModel product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }

                var result = _productHandler.CrearProducto(product);

                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creando producto");
            }
        }

        [HttpGet("~/api/ProductsByBusinessID")]
        public List<ProductModel> getProductsByBusinessID(int businessID)
        {
            var products = _productHandler.getProductsByBusinessID(Convert.ToString(businessID));
            return products;
        }
    }
}
