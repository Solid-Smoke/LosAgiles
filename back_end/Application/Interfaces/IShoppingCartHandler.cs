using back_end.Domain;
namespace back_end.Application.Interfaces
{
    public interface IShoppingCartHandler
    {
        List<ShoppingCartItemModel> GetCart(string clientId);
        bool DeleteCart(string clientId);
        public bool DeleteItemFromCart(string clientId, int productId);
        public List<ShoppingCartItemModel> ValidateCartQuantities(string clientId, List<ShoppingCartItemModel> cartItems);
    }
}