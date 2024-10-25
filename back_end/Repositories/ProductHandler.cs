using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace back_end.Repositories
{
    public class ProductHandler : IProductHandler
    {
        private readonly SqlConnection sqlConnection;

        public ProductHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        private void EjecutarComando(SqlCommand comando)
        {
            sqlConnection.Open();
            comando.ExecuteNonQuery();
            sqlConnection.Close();
        }

        private DataTable createTableResult(string query)
        {
            SqlCommand selectCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter tableAdapter = new SqlDataAdapter(selectCommand);
            DataTable resultTable = new DataTable();
            sqlConnection.Open();
            tableAdapter.Fill(resultTable);
            sqlConnection.Close();
            return resultTable;
        }

        public bool CrearProducto(ProductModel producto)
        {
            // Insertar producto
            string consultaProducto = @"
                INSERT INTO Products (Name, Description, Price, Stock, Weight, Perishable, DailyAmount, DaysAvailable, BusinessID)
                VALUES (@Name, @Description, @Price, @Stock, @Weight, @Perishable, @DailyAmount, @DaysAvailable, @BusinessID);
                SELECT SCOPE_IDENTITY();";

            SqlCommand comandoProducto = new SqlCommand(consultaProducto, sqlConnection);
            comandoProducto.Parameters.AddWithValue("@Name", producto.Name);
            comandoProducto.Parameters.AddWithValue("@Description", producto.Description);
            comandoProducto.Parameters.AddWithValue("@Price", producto.Price);
            comandoProducto.Parameters.AddWithValue("@Stock", producto.Stock);
            comandoProducto.Parameters.AddWithValue("@Weight", producto.Weight);
            comandoProducto.Parameters.AddWithValue("@Perishable", producto.Perishable);
            comandoProducto.Parameters.AddWithValue("@DailyAmount", (object)producto.DailyAmount ?? DBNull.Value);
            comandoProducto.Parameters.AddWithValue("@DaysAvailable", producto.DaysAvailable);
            comandoProducto.Parameters.AddWithValue("@BusinessID", producto.BusinessID);

            sqlConnection.Open();
            int productId = Convert.ToInt32(comandoProducto.ExecuteScalar());
            sqlConnection.Close();

            // Insertar imagen
            if (producto.ProductImage != null)
            {
                string consultaImagen = @"
                    INSERT INTO Images (Image, ProductID)
                    VALUES (@Image, @ProductID);";

                SqlCommand comandoImagen = new SqlCommand(consultaImagen, sqlConnection);
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

        private List<T> dapperSelectQuery<T>(string query, object parameters)
        {
            sqlConnection.Open();
            List<T> result = sqlConnection.Query<T>(query, parameters)
                             .ToList();
            sqlConnection.Close();
            return result;
        }

        private int dapperCountQuery(string query, object parameters)
        {
            sqlConnection.Open();
            int result = sqlConnection.Query<int>(query, parameters)
                             .ToList()[0];
            sqlConnection.Close();
            return result;
        }

        public List<ProductsSearchModel> searchProducts(string searchText,
                int startIndex, int maxResults)
        {
            string query = "SELECT Products.[Name], [Description], Price," +
                           "Businesses.Name AS BusinessName, ProductImage\r\n" +
                           "FROM Products LEFT JOIN Businesses\r\n" +
                           "ON Businesses.BusinessID = Products.BusinessID\r\n"+
                           "WHERE Products.[Name] LIKE @searchText\r\n" +
                           "OR Businesses.[Name] LIKE @searchText\r\n" +
                           "OR Products.Category LIKE @searchText\r\n" +
                           "ORDER BY Products.ProductID\r\n" +
                           "OFFSET @startIndex ROWS\r\n" +
                           "FETCH NEXT @maxResults ROWS ONLY";

            return dapperSelectQuery<ProductsSearchModel>(query,
                new
                {
                    searchText = "%" + searchText + "%",
                    startIndex,
                    maxResults
                });
        }

        public int countProductsBySearch(string searchText)
        {
            string query = "SELECT count(*)\r\n" +
                           "FROM Products LEFT JOIN Businesses\r\n" +
                           "ON Businesses.BusinessID = Products.BusinessID\r\n"+
                           "WHERE Products.[Name] LIKE @searchText\r\n" +
                           "OR Businesses.[Name] LIKE @searchText\r\n" +
                           "OR Products.Category LIKE @searchText\r\n";
            return dapperCountQuery(query,
                new { searchText = "%" + searchText + "%", });
        }
    }
}