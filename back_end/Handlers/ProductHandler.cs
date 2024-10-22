using back_end.Models;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Handlers
{
    public class ProductHandler
    {
        private readonly SqlConnection _conexion;
        private string? _rutaConexion;

        public ProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("ARQContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        private void EjecutarComando(SqlCommand comando)
        {
            _conexion.Open();
            comando.ExecuteNonQuery();
            _conexion.Close();
        }

        private DataTable createTableResult(string query)
        {
            SqlCommand selectCommand = new SqlCommand(query, _conexion);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(selectCommand);
            DataTable resultTable = new DataTable();
            _conexion.Open();
            tableAdapter.Fill(resultTable);
            _conexion.Close();
            return resultTable;
        }

        public bool CrearProducto(ProductModel producto)
        {
            string consultaProducto = @"
                INSERT INTO Products (Name, Description, Price, Stock, Weight, IsPerishable, DailyAmount, DaysAvailable, BusinessID, ProductImage)
                VALUES (@Name, @Description, @Price, @Stock, @Weight, @IsPerishable, @DailyAmount, @DaysAvailable, @BusinessID, @ProductImage);";

            SqlCommand comandoProducto = new SqlCommand(consultaProducto, _conexion);
            comandoProducto.Parameters.AddWithValue("@Name", producto.Name);
            comandoProducto.Parameters.AddWithValue("@Description", producto.Description);
            comandoProducto.Parameters.AddWithValue("@Price", producto.Price);
            comandoProducto.Parameters.AddWithValue("@Stock", producto.Stock);
            comandoProducto.Parameters.AddWithValue("@Weight", producto.Weight);
            comandoProducto.Parameters.AddWithValue("@IsPerishable", producto.IsPerishable);
            comandoProducto.Parameters.AddWithValue("@DailyAmount", (object)producto.DailyAmount ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@DaysAvailable", (object)producto.DaysAvailable ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@BusinessID", producto.BusinessID);
            comandoProducto.Parameters.AddWithValue("@ProductImage", (object)producto.ProductImage ?? DBNull.Value);

            _conexion.Open();
            comandoProducto.ExecuteNonQuery();
            _conexion.Close();

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
                        IsPerishable = Convert.ToBoolean(column["IsPerishable"]), // Cambiado de Perishable a IsPerishable
                        DailyAmount = column["DailyAmount"] as int?,
                        DaysAvailable = Convert.ToString(column["DaysAvailable"]),
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        ProductImage = column["ProductImage"] as byte[] // Imagen binaria
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
                        IsPerishable = Convert.ToBoolean(column["IsPerishable"]), // Cambiado de Perishable a IsPerishable
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
