using back_end.Application.Interfaces;
namespace back_end.Application.Commands {
    public class RejectOrder {
        private readonly IOrderHandler _orderHandler;

        public RejectOrder(IOrderHandler orderHandler) {
            _orderHandler = orderHandler;
        }

        public bool Execute(string OrderId) {
            return _orderHandler.RejectOrder(OrderId);
        }
    }
}
