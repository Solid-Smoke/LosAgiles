using back_end.Domain;

namespace back_end.Repositories
{
    public interface IProductHandler
    {
        int countProductsBySearch(string searchText);
        bool CrearProducto(ProductModel producto);
        List<ProductModel> getProductsByBusinessID(string businessID);
        List<ProductsSearchModel> searchProducts(string searchText, int startIndex, int maxResults);
    }
}