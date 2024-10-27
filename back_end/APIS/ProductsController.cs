using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using back_end.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductCommand _productCommand;
        private readonly IProductQuery productQuery;

        public ProductsController(IProductQuery productQuery, IProductHandler productHandler)
        {
            this.productQuery = productQuery;
            this._productCommand = new ProductCommand(productHandler);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateProduct([FromForm] ProductModel product)
        {
            try
            {
                var result = await _productCommand.createProduct(product, Request);
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
                var products = productQuery.getAllProducts();
                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo productos.");
            }
        }

        [HttpGet]
        public async
            Task<ActionResult<List<ProductsSearchModel>>> searchProducts(
            int startIndex, int maxResults, string? searchText)
        {
            return productQuery.
                searchProducts(startIndex, maxResults, searchText);
        }

        [HttpGet("CountProductsBySearch")]
        public async
            Task<ActionResult<int>> countProductsBySearch(string? searchText)
        {
            return productQuery.countProductsBySearch(searchText);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductModel> GetProductById(int id)
        {
            try
            {
                var product = _productQuery.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                return new JsonResult(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo producto.");
            }
        }
    }
}
