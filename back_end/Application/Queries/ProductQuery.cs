using back_end.Domain;
using back_end.Application.Interfaces;

namespace back_end.Application.Queries
{
    public interface IProductQuery
    {
        int countProductsBySearch(string? searchText);
        List<ProductModel> getAllProducts();
        ProductModel GetProductById(int id);
        List<ProductsSearchModel> searchProducts(int startIndex, int maxResults, string? searchText);
    }

    public class ProductQuery : IProductQuery
    {
        private readonly IProductHandler productHandler;

        public ProductQuery(IProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }

        public List<ProductModel> getAllProducts()
        {
            var products = productHandler.GetAllProducts();

            foreach (var product in products)
            {
                if (product.ProductImage != null)
                {
                    product.ProductImageBase64 = Convert.ToBase64String(product.ProductImage);
                }
            }

            return products;
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
        public ProductModel GetProductById(int id)
        {
            var product = productHandler.GetProductById(id);

            if (product.ProductImage != null)
            {
                product.ProductImageBase64 = Convert.ToBase64String(product.ProductImage);
            }

            return product;
        }
    }
}
