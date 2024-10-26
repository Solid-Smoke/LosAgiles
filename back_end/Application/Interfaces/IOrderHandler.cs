using back_end.Domain;

namespace back_end.Application.Interfaces {
    public interface IOrderHandler {
        List<OrderModel> GetPendingOrders();
        bool ApproveOrder(string OrderID);
        bool RejectOrder(string OrderID);
    }
}
