using back_end.Domain;

namespace back_end.Application.Interfaces {
    public interface IOrderHandler {
        List<OrderModel> GetPendingOrders();
        List<OrderModel> GetOrdersByClientID(string ClientID);
        List<OrderProductsModel> GetProductsByOrderID(string OrderID);
        bool ApproveOrder(string OrderID);
        bool RejectOrder(string OrderID);
        bool createOrder(CreateOrderModel orderData);
    }
}
