using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetOrdersExcludingCompleted
    {
        private readonly IOrderHandler _orderHandler;

        public GetOrdersExcludingCompleted(IOrderHandler orderHandler)
        {
            _orderHandler = orderHandler;
        }

        public List<OrderModel> Execute(int userID)
        {
            return _orderHandler.GetOrdersExcludingCompleted(userID);
        }
    }
}
