using back_end.Domain;

namespace back_end.Application.Interfaces
{
    public interface IProductHandler
    {
        int CountProductsBySearch(string searchText);
        bool CreateProduct(ProductModel product);
        List<ProductModel> GetAllProducts();
        ProductModel GetProductById(int id);
        List<InventoryItem> GetProductsByBusinessID(string businessID);
        List<ProductsSearchModel> SearchProducts(string searchText, int startIndex, int maxResults);
    }
}