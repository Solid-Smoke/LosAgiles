using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class DeleteInvalidProductsFromUserCart
    {
        private readonly IShoppingCartHandler _shoppingCartHandler;

        public DeleteInvalidProductsFromUserCart(IShoppingCartHandler shoppingCartHandler)
        {
            _shoppingCartHandler = shoppingCartHandler;
        }

        public void Execute(string clientId, List<ShoppingCartItemModel> itemsToDelete)
        {
            foreach (var item in itemsToDelete)
            {
                _shoppingCartHandler.DeleteItemFromCart(clientId, item.ProductID);
            }
        }
    }
}
