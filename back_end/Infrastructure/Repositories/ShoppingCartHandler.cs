using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using back_end.Application.Interfaces;

namespace back_end.Infrastructure.Repositories
{
    public class ShoppingCartHandler : IShoppingCartHandler
    {
        private SqlConnection sqlConnection;
        private string? routeConnection;

        public ShoppingCartHandler()
        {
            var builder = WebApplication.CreateBuilder();
            routeConnection = builder.Configuration.GetConnectionString("ClientsContext");
            sqlConnection = new SqlConnection(routeConnection);
        }

        public List<ShoppingCartItemModel> GetCart(string clientId)
        {
            List<ShoppingCartItemModel> cartData = new List<ShoppingCartItemModel>();
            string query = "SELECT * FROM [udfSelectShoppingCart](@ClientId)";
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientId", clientId);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartData.Add(
                                new ShoppingCartItemModel
                                {
                                    ProductID = Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    BusinessName = reader["BusinessName"].ToString(),
                                    Amount = Convert.ToInt32(reader["Amount"]),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    TotalSales = Convert.ToDecimal(reader["TotalSales"]),
                                    Weight = Convert.ToDecimal(reader["Weight"]),
                                    BusinessID = Convert.ToInt32(reader["BusinessID"]),
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
            return cartData;
        }

        public bool DeleteCart(string clientId)
        {
            string query = "DELETE FROM [ShoppingCarts] WHERE [ClientID] = @ClientId";
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientId", clientId);

                    sqlConnection.Open();
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return false;
            }
        }
    }
}