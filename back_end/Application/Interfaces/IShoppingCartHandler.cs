using back_end.Domain;
namespace back_end.Application.Interfaces
{
    public interface IShoppingCartHandler
    {
        List<ShoppingCartItemModel> GetCart(string clientId);
        bool DeleteCart(string clientId);
        public bool AddCartItem(string clientId, string productId, int amount);
    }
}