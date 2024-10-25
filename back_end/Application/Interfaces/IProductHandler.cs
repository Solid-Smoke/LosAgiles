using back_end.Domain;

namespace back_end.Application.Interfaces
{
    public interface IProductHandler
    {
        int countProductsBySearch(string searchText);
        bool CreateProduct(ProductModel product);
        List<ProductModel> GetAllProducts();
        List<ProductModel> getProductsByBusinessID(string businessID);
        List<ProductsSearchModel> searchProducts(string searchText, int startIndex, int maxResults);
    }
}
