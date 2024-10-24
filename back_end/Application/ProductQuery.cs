using back_end.Domain;
using back_end.Repositories;

namespace back_end.Application
{
    public class ProductQuery
    {
        private readonly ProductHandler _productHandler;

        public ProductQuery()
        {
            _productHandler = new ProductHandler();
        }

        public List<ProductModel> GetAllProducts()
        {
            var products = _productHandler.GetAllProducts();

            foreach (var product in products)
            {
                if (product.ProductImage != null)
                {
                    product.ProductImageBase64 = Convert.ToBase64String(product.ProductImage);
                }
            }

            return products;
        }
    }
}
