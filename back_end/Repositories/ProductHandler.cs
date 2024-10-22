using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Repositories
{
    public class ProductHandler
    {
        private readonly SqlConnection _connection;
        private string? _connectionRoute;

        public ProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionRoute = builder.Configuration.GetConnectionString("ARQContext");
            _connection = new SqlConnection(_connectionRoute);
        }

        private void RunCommand(SqlCommand command)
        {
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        private DataTable createTableResult(string query)
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
            string querryProduct = @"
                INSERT INTO Products (Name, Description, Price, Stock, Weight, IsPerishable, DailyAmount, DaysAvailable, BusinessID, ProductImage)
                VALUES (@Name, @Description, @Price, @Stock, @Weight, @IsPerishable, @DailyAmount, @DaysAvailable, @BusinessID, @ProductImage);";

            SqlCommand comandoProducto = new SqlCommand(querryProduct, _connection);
            comandoProducto.Parameters.AddWithValue("@Name", product.Name);
            comandoProducto.Parameters.AddWithValue("@Description", product.Description);
            comandoProducto.Parameters.AddWithValue("@Price", product.Price);
            comandoProducto.Parameters.AddWithValue("@Stock", product.Stock);
            comandoProducto.Parameters.AddWithValue("@Weight", product.Weight);
            comandoProducto.Parameters.AddWithValue("@IsPerishable", product.IsPerishable);
            comandoProducto.Parameters.AddWithValue("@DailyAmount", (object)product.DailyAmount ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@DaysAvailable", (object)product.DaysAvailable ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@BusinessID", product.BusinessID);
            comandoProducto.Parameters.AddWithValue("@ProductImage", (object)product.ProductImage ?? DBNull.Value);

            _connection.Open();
            comandoProducto.ExecuteNonQuery();
            _connection.Close();

            return true;
        }

        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            string query = "SELECT * FROM Products";
            DataTable tableQueryResult = createTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                products.Add(
                    new ProductModel
                    {
                        ProductID = Convert.ToInt32(column["ProductID"]),
                        Name = Convert.ToString(column["Name"]),
                        Description = Convert.ToString(column["Description"]),
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

        public List<ProductModel> getProductsByBusinessID(string businessID)
        {
            List<ProductModel> businessData = new List<ProductModel>();
            string query = "SELECT * FROM PRODUCTS WHERE BusinessID = " + businessID;
            DataTable tableQueryResult = createTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(
                    new ProductModel
                    {
                        ProductID = Convert.ToInt32(column["ProductID"]),
                        Name = Convert.ToString(column["Name"]),
                        Description = Convert.ToString(column["Description"]),
                        Price = Convert.ToInt32(column["Price"]),
                        Stock = Convert.ToInt32(column["Stock"]),
                        Weight = Convert.ToDecimal(column["Weight"]),
                        IsPerishable = Convert.ToBoolean(column["IsPerishable"]), 
                        DailyAmount = Convert.ToInt32(column["DailyAmount"]),
                        DaysAvailable = Convert.ToString(column["DaysAvailable"]),
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        ProductImage = (byte[])column["ProductImage"]
                    });
            }
            return businessData;
        }
    }
}
