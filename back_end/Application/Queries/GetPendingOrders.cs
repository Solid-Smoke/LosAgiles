using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
namespace back_end.Application.Queries {
    public class GetPendingOrders {
        private readonly IOrderHandler _orderHandler;

        public GetPendingOrders(IOrderHandler orderHandler) {
            _orderHandler = orderHandler;
        }

        public List<OrderModel> Execute() {
            return _orderHandler.GetPendingOrders();
        }
    }
}
