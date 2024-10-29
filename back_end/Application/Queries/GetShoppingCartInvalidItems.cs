using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetShoppingCartInvalidItems
    {
        private readonly IShoppingCartHandler _shoppingCartHandler;

        public GetShoppingCartInvalidItems(IShoppingCartHandler shoppingCartHandler)
        {
            _shoppingCartHandler = shoppingCartHandler;
        }

        public List<ShoppingCartItemDataModel> Execute(string clientId)
        {
            var cartItems = _shoppingCartHandler.GetCart(clientId);

            return _shoppingCartHandler.ValidateCartQuantities(clientId, cartItems);
        }
    }
}