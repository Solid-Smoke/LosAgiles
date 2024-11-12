using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using back_end.Application.Interfaces;
using Dapper;

namespace back_end.Infrastructure.Repositories {
    public class OrderHandler : IOrderHandler {
        private readonly SqlConnection sqlConnection;

        public OrderHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        private int substractProductsStock(List<CreateOrderProductsModel> products)
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

        public bool createOrder(CreateOrderModel orderData)
        {
            sqlConnection.Open();
            sqlConnection.Execute("BEGIN TRAN");
            try
            {
                int? orderID = insertInOrders(orderData.ClientID, orderData.DeliveryAddress, orderData.DeliveryDate);
                if (orderID == null)
                    throw new Exception("Inserted OrderID is null");
                Console.WriteLine("Inserted orderID in transaction: " + orderID);
                bool insertedInOrderProducts = insertInOrderProducts((int)orderID, orderData.Products);
                bool InsertedInBusinessOrders = insertInBusinessOrders((int)orderID, orderData.Products);
                int rowsAffected = substractProductsStock(orderData.Products);
                sqlConnection.Execute("COMMIT");
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                sqlConnection.Execute("ROLLBACK");
                sqlConnection.Close();
                throw new Exception(ex.ToString());
            }
        }

        private bool insertInBusinessOrders(int orderID, List<CreateOrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { orderID, product.BusinessID });
            insertList = insertList.Distinct<object>().ToList();
            return sqlConnection.Execute("EXEC spInsertBusinessOrders @orderIDParam = @orderID, @BusinessIDParam = @BusinessID",
                insertList) > 0;
        }

        private bool insertInOrderProducts(int orderID, List<CreateOrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { product.ProductID, orderID, product.Ammount });
            return sqlConnection.Execute("INSERT INTO OrderProducts (ProductId, OrderID, Amount) VALUES (@ProductId, @orderID, @Ammount)", insertList) > 0;
        }

        private int insertInOrders(int? clientID, ClientsAddress deliveryAddress, string deliveryDate)
        {
            
            return sqlConnection.QuerySingle<int>(
                "INSERT INTO Orders (CreatedDate, ClientID, DeliveryAddress, DeliveryDate)\r\n" +
                "OUTPUT INSERTED.OrderID\r\n" +
                "VALUES (getdate(), @clientID, @deliveryAddressID, @deliveryDate)",
                new { clientID, deliveryAddressID = deliveryAddress.AddressID, deliveryDate }
            );
        }

        public List<OrderModel> GetPendingOrders() {
            List<OrderModel> OrderData = new List<OrderModel>();
            string query = "SELECT * FROM [udfGetPendingOrders]()";
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
                                    ? Convert.ToDateTime(reader["DeliveryDate"]) : (DateTime?)null,
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
    }
}
