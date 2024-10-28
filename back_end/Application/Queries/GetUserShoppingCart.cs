using back_end.Application.Interfaces;
using back_end.Domain;
namespace back_end.Application.Queries
{
    public class GetUserShoppingCart
    {
        private readonly IShoppingCartHandler _shoppingCartHandler;

        public GetUserShoppingCart(IShoppingCartHandler shoppingCartHandler)
        {
            _shoppingCartHandler = shoppingCartHandler;
        }

        public List<ShoppingCartItemDataModel> Execute(string clientId)
        {
            return _shoppingCartHandler.GetCart(clientId);
        }
    }
}