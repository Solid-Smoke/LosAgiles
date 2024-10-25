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

        private void EjecutarComando(SqlCommand comando)
        {
            _connection.Open();
            comando.ExecuteNonQuery();
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

        public bool CrearProducto(ProductModel producto)
        {
            // Insertar producto
            string consultaProducto = @"
                INSERT INTO Products (Name, Description, Price, Stock, Weight, Perishable, DailyAmount, DaysAvailable, BusinessID)
                VALUES (@Name, @Description, @Price, @Stock, @Weight, @Perishable, @DailyAmount, @DaysAvailable, @BusinessID);
                SELECT SCOPE_IDENTITY();";

            SqlCommand comandoProducto = new SqlCommand(consultaProducto, _connection);
            comandoProducto.Parameters.AddWithValue("@Name", producto.Name);
            comandoProducto.Parameters.AddWithValue("@Description", producto.Description);
            comandoProducto.Parameters.AddWithValue("@Price", producto.Price);
            comandoProducto.Parameters.AddWithValue("@Stock", producto.Stock);
            comandoProducto.Parameters.AddWithValue("@Weight", producto.Weight);
            comandoProducto.Parameters.AddWithValue("@Perishable", producto.Perishable);
            comandoProducto.Parameters.AddWithValue("@DailyAmount", (object)producto.DailyAmount ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@DaysAvailable", producto.DaysAvailable);
            comandoProducto.Parameters.AddWithValue("@BusinessID", producto.BusinessID);

            _connection.Open();
            int productId = Convert.ToInt32(comandoProducto.ExecuteScalar());
            _connection.Close();

            // Insertar imagen
            if (producto.ProductImage != null)
            {
                string consultaImagen = @"
                    INSERT INTO Images (Image, ProductID)
                    VALUES (@Image, @ProductID);";

                SqlCommand comandoImagen = new SqlCommand(consultaImagen, _connection);
                comandoImagen.Parameters.AddWithValue("@Image", producto.ProductImage);
                comandoImagen.Parameters.AddWithValue("@ProductID", productId);

                EjecutarComando(comandoImagen);
            }

            return true;
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
                        Perishable = Convert.ToBoolean(column["Perishable"]),
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