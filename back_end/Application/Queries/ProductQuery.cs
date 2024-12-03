using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public interface IProductQuery
    {
        int CountProductsBySearch(string? searchText);
        List<ProductModel> getAllProducts();
        ProductModel GetProductById(int id);
        List<ProductsSearchModel> SearchProducts(int startIndex, int maxResults, string? searchText);
        public List<InventoryItem> GetProductsByBusinessID(string businessID);
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

        public List<ProductsSearchModel> SearchProducts(
            int startIndex, int maxResults, string? searchText)
        {
            if (startIndex < 0)
                throw new ArgumentException("startIndex must be greater than or equal to 0");
            if (searchText == null)
                searchText = "";
            var products = productHandler.SearchProducts(searchText, startIndex,
                maxResults);
            foreach (var product in products)
                if (product.ProductImage != null)
                    product.ProductImageBase64 = Convert.ToBase64String(
                    product.ProductImage);
            return products;
        }

        public int CountProductsBySearch(string? searchText)
        {
            if (searchText == null)
                searchText = "";
            return productHandler.CountProductsBySearch(searchText);
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

        public List<InventoryItem> GetProductsByBusinessID(string businessID)
        {
            return productHandler.GetProductsByBusinessID(businessID);
        }
    }
}
