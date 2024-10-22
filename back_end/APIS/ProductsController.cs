using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace back_end.Controllers
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
        public async Task<ActionResult<bool>> CreateProduct([FromForm] ProductModel product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    if (file != null && file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            product.ProductImage = memoryStream.ToArray();
                        }

                    }
                }

                var result = _productHandler.CreateProduct(product);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creando producto");
            }
        }

        [HttpGet("GetAllProducts")]
        public ActionResult<List<ProductModel>> GetAllProducts()
        {
            try
            {
                var products = _productHandler.GetAllProducts();

                foreach (var product in products)
                {
                    if (product.ProductImage != null)
                    {
                        product.ProductImageBase64 = Convert.ToBase64String(product.ProductImage);
                    }
                }

                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo productos");
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
