using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
namespace back_end.Application.Queries {
    public class GetOrdersByClientID {
        private readonly IOrderHandler _orderHandler;

        public GetOrdersByClientID(IOrderHandler orderHandler) {
            _orderHandler = orderHandler;
        }

        public List<OrderModel> Execute(string ClientId) {
            return _orderHandler.GetOrdersByClientID(ClientId);
        }
    }
}
