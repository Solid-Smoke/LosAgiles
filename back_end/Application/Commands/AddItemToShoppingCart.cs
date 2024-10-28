using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class AddItemToShoppingCart
    {
        private readonly IShoppingCartHandler _shoppingCartHandler;

        public AddItemToShoppingCart(IShoppingCartHandler shoppingCartHandler)
        {
            _shoppingCartHandler = shoppingCartHandler;
        }

        public bool Execute(string clientId, string productId, int amount)
        {
            return _shoppingCartHandler.AddCartItem(clientId, productId, amount);
        }
    }
}
