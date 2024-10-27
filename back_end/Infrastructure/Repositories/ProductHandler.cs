using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using back_end.Application.Interfaces;

namespace back_end.Infrastructure.Repositories
{

    public class ProductHandler : IProductHandler
    {
        private readonly SqlConnection sqlConnection;

        public ProductHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
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
                        IsPerishable = Convert.ToBoolean(column["Perishable"]),
                        DailyAmount = Convert.ToInt32(column["DailyAmount"]),
                        DaysAvailable = Convert.ToString(column["DaysAvailable"]),
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        ProductImage = (byte[])column["ProductImage"]
                    });
            }
            return businessData;
        }

        private void runCommand(SqlCommand command)
        {
            sqlConnection.Open();
            command.ExecuteNonQuery();
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

        public bool CreateProduct(ProductModel product)
        {
            string queryProduct = @"
                INSERT INTO Products (Name, Description, Category, Price, Stock, Weight, IsPerishable, DailyAmount, DaysAvailable, BusinessID, ProductImage)
                VALUES (@Name, @Description, @Category, @Price, @Stock, @Weight, @IsPerishable, @DailyAmount, @DaysAvailable, @BusinessID, @ProductImage);"
            ;

            SqlCommand commandProduct = new SqlCommand(queryProduct, sqlConnection);
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

            runCommand(commandProduct);
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
            string query = "SELECT Products.ProductID, " +
                           "Products.[Name], [Description], Price," +
                           "Businesses.Name AS BusinessName, ProductImage\r\n" +
                           "FROM Products LEFT JOIN Businesses\r\n" +
                           "ON Businesses.BusinessID = Products.BusinessID\r\n" +
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
                           "ON Businesses.BusinessID = Products.BusinessID\r\n" +
                           "WHERE Products.[Name] LIKE @searchText\r\n" +
                           "OR Businesses.[Name] LIKE @searchText\r\n" +
                           "OR Products.Category LIKE @searchText\r\n";
            return dapperCountQuery(query,
                new { searchText = "%" + searchText + "%", });
        }
        public ProductModel GetProductById(int id)
        {
            string query = $@"
        SELECT p.*, b.Name AS BusinessName 
        FROM Products p
        INNER JOIN Businesses b ON p.BusinessID = b.BusinessID 
        WHERE ProductID = {id}";

            DataTable tableQueryResult = CreateTableResult(query);

            if (tableQueryResult.Rows.Count == 0)
            {
                return null;
            }

            DataRow column = tableQueryResult.Rows[0];

            return new ProductModel
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
                BusinessName = Convert.ToString(column["BusinessName"]),
                ProductImage = column["ProductImage"] as byte[]
            };
        }

    }
}