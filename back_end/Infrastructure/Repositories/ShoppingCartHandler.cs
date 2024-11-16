using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data.SqlClient;

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

        public List<ShoppingCartItemDataModel> GetCart(string clientId)
        {
            List<ShoppingCartItemDataModel> cartData = new List<ShoppingCartItemDataModel>();
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
                                new ShoppingCartItemDataModel
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

        public bool DeleteItemFromCart(string clientId, int productId)
        {
            string query = "DELETE FROM [ShoppingCarts] WHERE [ClientID] = @ClientId AND [ProductID] = @ProductId";
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientId", clientId);
                    sqlCommand.Parameters.AddWithValue("@ProductId", productId);

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

        public bool AddCartItem(string clientId, ShoppingCartItemModel newItem)
        {
            string query = "INSERT INTO [ShoppingCarts] ([ClientID], [ProductID], [Amount]) VALUES (@ClientId, @ProductID, @Amount)";
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientId", clientId);
                    sqlCommand.Parameters.AddWithValue("@ProductID", newItem.ProductID);
                    sqlCommand.Parameters.AddWithValue("@Amount", newItem.Amount);

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

        public List<ShoppingCartItemDataModel> ValidateCartQuantities(string clientId, List<ShoppingCartItemDataModel> cartItems)
        {
            string query = @"
                SELECT [p].[ProductID], [p].[Stock]
                FROM [Products] [p]
                JOIN [ShoppingCarts] [sc] ON [p].[ProductID] = [sc].ProductID
                WHERE [sc].ClientID = @ClientId AND IsDeleted = 0";

            List<ShoppingCartItemDataModel> invalidProducts = new List<ShoppingCartItemDataModel>();

            try
            {
                Dictionary<int, int> productStock = new Dictionary<int, int>();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientId", clientId);
                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = Convert.ToInt32(reader["ProductID"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            productStock[productId] = stock;
                        }
                    }
                    sqlConnection.Close();
                }

                foreach (var item in cartItems)
                {
                    if (productStock.ContainsKey(item.ProductID))
                    {
                        int stock = productStock[item.ProductID];
                        if (item.Amount > stock)
                        {
                            invalidProducts.Add(item);
                        }
                    }
                }

                return invalidProducts;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return invalidProducts;
            }
        }
    }
}