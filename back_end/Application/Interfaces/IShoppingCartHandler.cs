using back_end.Domain;
namespace back_end.Application.Interfaces
{
    public interface IShoppingCartHandler
    {
        List<ShoppingCartItemDataModel> GetCart(string clientId);
        bool DeleteCart(string clientId);
        public bool DeleteItemFromCart(string clientId, int productId);
        public List<ShoppingCartItemDataModel> ValidateCartQuantities(string clientId, List<ShoppingCartItemDataModel> cartItems);
        public bool AddCartItem(string clientId, ShoppingCartItemModel newItem);
    }
}