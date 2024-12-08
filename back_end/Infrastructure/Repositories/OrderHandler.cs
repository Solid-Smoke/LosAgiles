﻿using back_end.Application.Interfaces;
using back_end.Domain;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace back_end.Infrastructure.Repositories {
    public class OrderHandler : IOrderHandler {
        private readonly SqlConnection sqlConnection;

        public OrderHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        private int SubstractProductsStock(List<CreateOrderProductsModel> products)
        {
            int rowsAffected = 0;
            foreach (var product in products)
                rowsAffected += sqlConnection.Execute("UPDATE Products\r\n" +
                    "SET Products.Stock = (select Stock from Products where ProductID = @ProductID) - @Ammount\r\n" +
                    "WHERE ProductID = @ProductID",
                    new { product.ProductID, product.Ammount }
                );
            return rowsAffected;
        }

        private void CheckIfProductWasSoftDeleted(List<CreateOrderProductsModel> products)
        {
            foreach (var product in products)
            {
                int rowsSelected = sqlConnection.Query<int>("SELECT ProductID FROM Products WHERE ProductID = @ProductID AND IsDeleted = 1",
                    new { product.ProductID }).ToList().Count;
                if (rowsSelected > 0)
                    throw new Exception("A product was deleted (soft delete) while creating the order, submit order aborted");
            }
        }

        public bool CreateOrder(CreateOrderModel orderData)
        {
            sqlConnection.Open();
            sqlConnection.Execute("BEGIN TRAN");
            try
            {
                int? orderID = InsertInOrders(orderData);
                if (orderID == null)
                    throw new Exception("Inserted OrderID is null");
                bool insertedInOrderProducts = InsertInOrderProducts((int)orderID, orderData.Products);
                bool InsertedInBusinessOrders = InsertInBusinessOrders((int)orderID, orderData.Products);
                int rowsAffected = SubstractProductsStock(orderData.Products);
                CheckIfProductWasSoftDeleted(orderData.Products);
                sqlConnection.Execute("COMMIT");
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                sqlConnection.Execute("ROLLBACK");
                sqlConnection.Close();
                throw;
            }
        }

        private bool InsertInBusinessOrders(int orderID, List<CreateOrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { orderID, product.BusinessID });
            insertList = insertList.Distinct<object>().ToList();
            return sqlConnection.Execute("EXEC spInsertBusinessOrders @orderIDParam = @orderID, @BusinessIDParam = @BusinessID",
                insertList) > 0;
        }

        private bool InsertInOrderProducts(int orderID, List<CreateOrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { product.ProductID, orderID, product.Ammount });
            int rowsAffected = sqlConnection.Execute("INSERT INTO OrderProducts (ProductId, OrderID, Amount) VALUES (@ProductId, @orderID, @Ammount)", insertList);
            if (rowsAffected != products.Count)
                throw new Exception("A product of the order was deleted while saving the order products data");
            else
                return true;
        }

        private int InsertInOrders(CreateOrderModel orderData)
        {

            return sqlConnection.QuerySingle<int>(
                "INSERT INTO Orders (CreatedDate, ClientID, DeliveryAddress, DeliveryDate, DeliveryCost, SubtotalCost, TotalCost)\r\n" +
                "OUTPUT INSERTED.OrderID\r\n" +
                "VALUES (getdate(), @clientID, @deliveryAddressID, @deliveryDate, @deliveryCost, @subtotalCost, @totalCost)",
                new
                {
                    orderData.ClientID,
                    deliveryAddressID = orderData.DeliveryAddress.AddressID,
                    orderData.DeliveryDate,
                    orderData.DeliveryCost,
                    orderData.SubtotalCost,
                    orderData.TotalCost
                }
            );
        }

        public List<OrderModel> GetPendingOrders() {
            List<OrderModel> OrderData = new List<OrderModel>();
            string query = "SELECT * FROM [udfGetOrdersByStatus]('Pendiente')";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
                        while (reader.Read()) {
                            OrderData.Add(
                                new OrderModel {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    Buyer = reader["Buyer"].ToString(),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    Address = reader["Address"].ToString()
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return OrderData;
        }

        public List<OrderModel> GetApprovedOrders() {
            List<OrderModel> OrderData = new List<OrderModel>();
            string query = "SELECT * FROM [udfGetOrdersByStatus]('Aprobada')";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
                        while (reader.Read()) {
                            OrderData.Add(
                                new OrderModel {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    Buyer = reader["Buyer"].ToString(),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    Address = reader["Address"].ToString()
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return OrderData;
        }

        public List<OrderModel> GetOrdersByClientID(string clientID) {
            List<OrderModel> OrderData = new List<OrderModel>();
            string query = "SELECT * FROM udfOrdersByClientID(@ClientID);";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlCommand.Parameters.AddWithValue("@ClientID", clientID);
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
                        while (reader.Read()) {
                            OrderData.Add(
                                new OrderModel {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    DeliveryDate = reader["DeliveryDate"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["DeliveryDate"]) : null,
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    Status = reader["Status"].ToString(),
                                    Address = reader["Address"].ToString()
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return OrderData;
        }

        public List<OrderProductsModel> GetProductsByOrderID(string orderID) {
            List<OrderProductsModel> OrderProductsData = new List<OrderProductsModel>();
            string query = "SELECT * FROM [udfProductsByOrderID](@OrderID)";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlCommand.Parameters.AddWithValue("@OrderID", orderID);
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
                        while (reader.Read()) {
                            OrderProductsData.Add(
                                new OrderProductsModel {
                                    Amount = Convert.ToInt32(reader["Amount"]),
                                    ProductName = reader["Name"].ToString()
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return OrderProductsData;
        }

        public bool ApproveOrder(string orderID) {
            string query = "UPDATE [Orders] SET [Status] = 'Aprobada' WHERE [OrderId] = @OrderID";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlCommand.Parameters.AddWithValue("@OrderID", orderID);

                    sqlConnection.Open();
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return false;
            }
        }

        public bool RejectOrder(string orderID) {
            string query = "UPDATE [Orders] SET [Status] = 'Rechazada' WHERE [OrderId] = @OrderID";
            try {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) {
                    sqlCommand.Parameters.AddWithValue("@OrderID", orderID);

                    sqlConnection.Open();
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException sqlEx) {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return false;
            }
        }

        public List<OrderModel> GetOrdersExcludingCompleted(int userID)
        {
            List<OrderModel> orders = new List<OrderModel>();
            string query = @"
                SELECT TOP 10
                    OrderID, 
                    Status, 
                    TotalCost
                FROM Orders
                WHERE Status != 'Completada' AND Status != 'Rechazada' AND ClientID = @UserID ORDER BY CreatedDate ASC";

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@UserID", userID);
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(
                                new OrderModel
                                {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    Status = reader["Status"].ToString(),
                                    TotalAmount = Convert.ToDecimal(reader["TotalCost"])
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return orders;
        }

        public List<OrderProductsModel> GetLastTenPurchased(int userID)
        {
            List<OrderProductsModel> lastTenProducts = new List<OrderProductsModel>();
            string query = @"
                SELECT TOP 10 
                    p.ProductID, 
                    p.Name AS ProductName, 
                    op.Amount, 
                    p.Price
                FROM OrderProducts op
                INNER JOIN Orders o ON op.OrderID = o.OrderID
                INNER JOIN Products p ON op.ProductID = p.ProductID
                WHERE o.ClientID = @UserID
                ORDER BY o.CreatedDate DESC";

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@UserID", userID);
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastTenProducts.Add(
                                new OrderProductsModel
                                {
                                    ProductID = Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Amount = Convert.ToInt32(reader["Amount"]),
                                    Price = Convert.ToDecimal(reader["Price"])
                                });
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return lastTenProducts;
        }

    }
}
