using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Application
{
    public interface IProductSearchLogic
    {
        List<ProductModel> searchProducts(string searchText, int startIndex, int maxResults, string filterTypeString, string filter);
    }

    public class ProductSearchLogic : IProductSearchLogic
    {
        private readonly IProductHandler productHandler;
        private readonly IFactoryProductSearchFilter factoryProductSearchFilter;

        public ProductSearchLogic(IProductHandler productHandler, IFactoryProductSearchFilter factoryProductSearchFilter)
        {
            this.productHandler = productHandler;
            this.factoryProductSearchFilter = factoryProductSearchFilter;
        }


        public List<ProductModel> searchProducts(string searchText,
            int startIndex, int maxResults, string filterTypeString, string filter)
        {
            return productHandler.searchProducts(searchText, startIndex,
                maxResults,
                factoryProductSearchFilter.getFilterType(filterTypeString), filter);
        }
    }
}
