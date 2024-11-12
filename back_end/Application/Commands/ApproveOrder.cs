using back_end.Application.Interfaces;
namespace back_end.Application.Commands {
    public class ApproveOrder {
        private readonly IOrderHandler _orderHandler;

        public ApproveOrder(IOrderHandler orderHandler) {
            _orderHandler = orderHandler;
        }

        public bool Execute(string OrderId) {
            return _orderHandler.ApproveOrder(OrderId);
        }
    }
}
