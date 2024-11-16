using back_end.Application.Commands;
using back_end.Application.Interfaces;
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
        private readonly ProductDelete _productDeleteCommand;
        private readonly IProductQuery productQuery;

        public ProductsController(IProductQuery productQuery, IProductHandler productHandler)
        {
            this.productQuery = productQuery;
            this._productDeleteCommand = new ProductDelete(productHandler);
            this._productCommand = new ProductCommand(productHandler);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateProduct([FromForm] ProductModel product)
        {
            try
            {
                var result = await _productCommand.CreateProduct(product, Request);
                return new JsonResult(result);
            }
            catch (Exception)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo productos.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductsSearchModel>>> SearchProducts(
            int startIndex, int maxResults, string? searchText)
        {
            return productQuery.
                SearchProducts(startIndex, maxResults, searchText);
        }

        [HttpGet("CountProductsBySearch")]
        public async Task<ActionResult<int>> CountProductsBySearch(string? searchText)
        {
            return productQuery.CountProductsBySearch(searchText);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductModel> GetProductById(int id)
        {
            try
            {
                var product = productQuery.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                return new JsonResult(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo producto.");
            }
        }

        [HttpGet("Business/{businessID}")]
        public ActionResult<List<ProductModel>> GetProductsByBusinessID(string businessID)
        {
            try
            {
                var products = productQuery.GetProductsByBusinessID(businessID);
                if (products == null || products.Count == 0)
                {
                    return NotFound("No se encontraron productos para el Business ID especificado.");
                }

                return new JsonResult(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo productos por Business ID.");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProducts([FromBody] List<int> productsIds)
        {
            try
            {
                return _productDeleteCommand.DeleteProducts(productsIds);
            }
            catch (Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, "Error eliminando productos.");
            }
        }
    }
}
