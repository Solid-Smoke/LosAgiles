using back_end.Application.Interfaces;
namespace back_end.Application.Commands
{
    public class DeleteUserShoppingCart
    {
        private readonly IShoppingCartHandler _shoppingCartHandler;

        public DeleteUserShoppingCart(IShoppingCartHandler shoppingCartHandler)
        {
            _shoppingCartHandler = shoppingCartHandler;
        }

        public bool Execute(string clientId)
        {
            return _shoppingCartHandler.DeleteCart(clientId);
        }
    }
}
