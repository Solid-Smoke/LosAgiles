using back_end.Application;
using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace back_end.Repositories
{
    public interface IProductHandler
    {
        public bool CrearProducto(ProductModel producto);
        public List<ProductModel> getProductsByBusinessID(string businessID);
        public List<ProductsSearchModel> searchProducts(string searchText,
            int startIndex, int maxResults, IProductSearchFilter filterType,
            string filter);
    }
    public class ProductHandler: IProductHandler
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
            sqlConnection.InfoMessage += connection_InfoMessage;
            List<T> result = sqlConnection.Query<T>(query, parameters)
                             .ToList();
            sqlConnection.Close();
            return result;
        }

        static void connection_InfoMessage(object sender, SqlInfoMessageEventArgs sqlEvent)
        {
            var outputFromStoredProcedure = sqlEvent.Message;
            Console.WriteLine(sqlEvent.Message);
        }

        public List<ProductsSearchModel> searchProducts(string searchText,
                int startIndex, int maxResults, IProductSearchFilter filterType,
                string filter)
        {
            string query = ellaborateSearchProductsQuery(filterType);

            var parametersValues = new DynamicParameters();
            parametersValues.Add("@searchText", "%" + filterType.parseSearchText(searchText) + "%");
            parametersValues.Add("@startIndex", startIndex);
            parametersValues.Add("@maxResults", maxResults);

            return dapperSelectQuery<ProductsSearchModel>(query, parametersValues);
        }

        private string ellaborateSearchProductsQuery(IProductSearchFilter filterType)
        {
            string query = "select Products.[Name], [Description], Price," +
                           "Businesses.Name, [Image]\r\n" +
                           "from Products left join Businesses\r\n" +
                           "on Businesses.BusinessID = Products.BusinessID\r\n" +
                           "where Products.[Name] like @searchText ";

            query += filterType.getQuery();

            query += " order by Products.ProductID\r\n" +
                     "OFFSET @startIndex ROWS\r\n" +
                     "FETCH NEXT @maxResults ROWS ONLY";
            query += " print @searchText print @startIndex print @maxResults";
            return query;
        }
    }
}