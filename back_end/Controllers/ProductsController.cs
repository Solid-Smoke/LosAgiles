using back_end.Application;
using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductHandler _productHandler;

        private readonly IProductSearchHttpLogic productHttpLogic;

        public ProductsController(IProductHandler productHandler,
            IProductSearchHttpLogic productHttpLogic)
        {
            this._productHandler = productHandler;
            this.productHttpLogic = productHttpLogic;
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

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ProductModel>>> searchProducts(
            string searchText, int startIndex, int maxResults,
            string filterTypeString, string filterInput)
        {
            return productHttpLogic.
                searchProducts(searchText, startIndex, maxResults,
                filterTypeString, filterInput);
        }
    }
}
