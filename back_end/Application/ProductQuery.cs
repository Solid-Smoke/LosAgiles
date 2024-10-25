using back_end.Repositories;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Application
{
    public interface IProductQuery
    {
        int countProductsBySearch(string? searchText);
        List<ProductsSearchModel> searchProducts(int startIndex, int maxResults, string? searchText);
    }

    public class ProductQuery : IProductQuery
    {
        private readonly IProductHandler productHandler;

        public ProductQuery(IProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }


        public List<ProductsSearchModel> searchProducts(
            int startIndex, int maxResults, string? searchText)
        {
            if (searchText == null)
                searchText = "";
            var products = productHandler.searchProducts(searchText, startIndex,
                maxResults);
            foreach (var product in products)
                if (product.ProductImage != null)
                    product.ProductImageInBase64 = Convert.ToBase64String(
                    product.ProductImage);
            return products;
        }

        public int countProductsBySearch(string? searchText)
        {
            if (searchText == null)
                searchText = "";
            return productHandler.countProductsBySearch(searchText);
        }
    }
}
