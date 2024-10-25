using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Application
{
    public interface IProductSearchHttpLogic
    {
        ActionResult<List<ProductModel>> searchProducts(string searchText, int startIndex, int maxResults, string? filterType, string? filterInput);
    }
   
    public class ProductSearchHttpLogic : ControllerBase,
        IProductSearchHttpLogic
    {
        private readonly IProductSearchLogic _productSearchLogic;
        private readonly IProductSearchHttpRequestParameterValidator
            productHttpRequestParameterValidator;

        public ProductSearchHttpLogic(IProductSearchLogic productSearchLogic,
            IProductSearchHttpRequestParameterValidator
            productSearchHttpRequestParameterValidator)
        {
            this._productSearchLogic = productSearchLogic;
            this.productHttpRequestParameterValidator =
                productSearchHttpRequestParameterValidator;
        }


        public ActionResult<List<ProductModel>> searchProducts(
           string searchText, int startIndex, int maxResults,
           string? filterType, string? filterInput)
        {
            try
            {
                productHttpRequestParameterValidator
                    .checkSearchProductsParameters(searchText,
                    startIndex, maxResults, filterType, filterInput);
                return new JsonResult(_productSearchLogic
                    .searchProducts(searchText,
                     startIndex, maxResults, filterType, filterInput));
            }
            catch (Exception ex)
            {
                if (ex.TargetSite == typeof(ProductSearchHttpRequestParameterValidator)
                    .GetMethod("checkSearchProductsParameters"))
                    return BadRequest(ex.Message);
                else
                    Console.WriteLine(ex.Message, ex.TargetSite);
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Uknown server error happened");
            }
        }
    }
}