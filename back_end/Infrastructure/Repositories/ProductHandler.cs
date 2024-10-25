using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Infrastructure.Repositories
{
    public class ProductHandler
    {
        private readonly SqlConnection _connection;
        private string? _connectionRoute;

        public ProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionRoute = builder.Configuration.GetConnectionString("ClientsContext");
            _connection = new SqlConnection(_connectionRoute);
        }

        private void RunCommand(SqlCommand command)
        {
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        private DataTable CreateTableResult(string query)
        {
            SqlCommand selectCommand = new SqlCommand(query, _connection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(selectCommand);
            DataTable resultTable = new DataTable();
            _connection.Open();
            tableAdapter.Fill(resultTable);
            _connection.Close();
            return resultTable;
        }

        public bool CreateProduct(ProductModel product)
        {
            string queryProduct = @"
                INSERT INTO Products (Name, Description, Category, Price, Stock, Weight, IsPerishable, DailyAmount, DaysAvailable, BusinessID, ProductImage)
                VALUES (@Name, @Description, @Category, @Price, @Stock, @Weight, @IsPerishable, @DailyAmount, @DaysAvailable, @BusinessID, @ProductImage);";

            SqlCommand commandProduct = new SqlCommand(queryProduct, _connection);
            commandProduct.Parameters.AddWithValue("@Name", product.Name);
            commandProduct.Parameters.AddWithValue("@Description", product.Description);
            commandProduct.Parameters.AddWithValue("@Category", product.Category);
            commandProduct.Parameters.AddWithValue("@Price", product.Price);
            commandProduct.Parameters.AddWithValue("@Stock", product.Stock);
            commandProduct.Parameters.AddWithValue("@Weight", product.Weight);
            commandProduct.Parameters.AddWithValue("@IsPerishable", product.IsPerishable);
            commandProduct.Parameters.AddWithValue("@DailyAmount", (object)product.DailyAmount ?? DBNull.Value);
            commandProduct.Parameters.AddWithValue("@DaysAvailable", (object)product.DaysAvailable ?? DBNull.Value);
            commandProduct.Parameters.AddWithValue("@BusinessID", product.BusinessID);
            commandProduct.Parameters.AddWithValue("@ProductImage", (object)product.ProductImage ?? DBNull.Value);

            RunCommand(commandProduct);
            return true;
        }

        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            string query = "SELECT * FROM Products";
            DataTable tableQueryResult = CreateTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                products.Add(
                    new ProductModel
                    {
                        ProductID = Convert.ToInt32(column["ProductID"]),
                        Name = Convert.ToString(column["Name"]),
                        Description = Convert.ToString(column["Description"]),
                        Category = Convert.ToString(column["Category"]),
                        Price = Convert.ToInt32(column["Price"]),
                        Stock = Convert.ToInt32(column["Stock"]),
                        Weight = Convert.ToDecimal(column["Weight"]),
                        IsPerishable = Convert.ToBoolean(column["IsPerishable"]),
                        DailyAmount = column["DailyAmount"] as int?,
                        DaysAvailable = Convert.ToString(column["DaysAvailable"]),
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        ProductImage = column["ProductImage"] as byte[]
                    });
            }
            return products;
        }
    }
}
