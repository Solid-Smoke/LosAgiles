using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using back_end.Application.Interfaces;

namespace back_end.Infrastructure.Repositories {
    public class OrderHandler : IOrderHandler {
        private SqlConnection sqlConnection;
        private string? routeConnection;
        public OrderHandler() {
            var builder = WebApplication.CreateBuilder();
            routeConnection = builder.Configuration.GetConnectionString("ClientsContext");
            sqlConnection = new SqlConnection(routeConnection);
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

        public bool ApproveOrder(string OrderID) {
            return true; //TODO
        }

        public bool RejectOrder(string OrderID) {
            return true; //TODO
        }
    }
}
