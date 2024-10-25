using back_end.Application.Commands;
using back_end.Application.Queries;
﻿using back_end.Application;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductCommand _productCommand;
        private readonly ProductQuery _productQuery;

        private readonly IProductQuery productQuery;

        public ProductsController(IProductHandler productHandler,
            IProductQuery productQuery)
        {
            this._productHandler = productHandler;
            this.productQuery = productQuery;
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
        [HttpGet]
        public async
            Task<ActionResult<List<ProductsSearchModel>>> searchProducts(
            int startIndex, int maxResults, string? searchText)
        {
            return productQuery.
                searchProducts(startIndex, maxResults, searchText);
        }
    }
}
