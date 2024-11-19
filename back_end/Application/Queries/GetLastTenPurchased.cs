using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetLastTenPurchased
    {
        private readonly IOrderHandler _orderHandler;

        public GetLastTenPurchased(IOrderHandler orderHandler)
        {
            _orderHandler = orderHandler;
        }

        public List<OrderProductsModel> Execute(int userID)
        {
            return _orderHandler.GetLastTenPurchased(userID);
        }
    }
}
