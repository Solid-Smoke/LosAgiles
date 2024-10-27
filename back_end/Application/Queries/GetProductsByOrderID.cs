using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
namespace back_end.Application.Queries {
    public class GetProductsByOrderID {
        private readonly IOrderHandler _orderHandler;

        public GetProductsByOrderID(IOrderHandler orderHandler) {
            _orderHandler = orderHandler;
        }

        public List<OrderProductsModel> Execute(string OrderId) {
            return _orderHandler.GetProductsByOrderID(OrderId);
        }
    }
}
