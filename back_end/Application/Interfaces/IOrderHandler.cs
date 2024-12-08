﻿using back_end.Domain;
using System.Data;

namespace back_end.Application.Interfaces
{
    public interface IOrderHandler
    {
        List<OrderModel> GetPendingOrders();
        List<OrderModel> GetApprovedOrders();
        List<OrderModel> GetOrdersByClientID(string ClientID);
        List<OrderProductsModel> GetProductsByOrderID(string OrderID);
        List<OrderModel> GetOrdersExcludingCompleted(int userID);
        List<OrderProductsModel> GetLastTenPurchased(int userID);

        bool ApproveOrder(string OrderID);
        bool RejectOrder(string OrderID);
        bool CreateOrder(CreateOrderModel orderData);
    }
}
