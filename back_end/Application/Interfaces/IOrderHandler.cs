using back_end.Domain;
using System.Data;

namespace back_end.Application.Interfaces {
    public interface IOrderHandler {
        List<OrderModel> GetPendingOrders();
        List<OrderModel> GetOrdersByClientID(string ClientID);
        List<OrderProductsModel> GetProductsByOrderID(string OrderID);
        bool ApproveOrder(string OrderID);
        bool RejectOrder(string OrderID);
        bool CreateOrder(CreateOrderModel orderData);
        public bool GetReportOrderData(string query, ReportBaseFilters baseFilters, out DataTable reportData);
        public bool GetOrderProductsData(string orderIDs, out DataTable queryResultTable);
    }
}
