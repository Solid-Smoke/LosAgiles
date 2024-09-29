using back_end.Models;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Handlers
{
    public class BusinessHandler
    {
        private SqlConnection _connection;
        private string? _routeConnection;

        public BusinessHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("ARQContext");
            _connection = new SqlConnection(_routeConnection);
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

        public List<BusinessModel> getAllBusiness()
        {
            List<BusinessModel> businessData = new List<BusinessModel>();
            string query = "SELECT * FROM dbo.Businesses";
            DataTable tableQueryResult = createTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(
                    new BusinessModel
                    {
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        Name = Convert.ToString(column["Name"]),
                        IDNumber = Convert.ToString(column["IDNumber"]),
                        Email = Convert.ToString(column["Email"]),
                        Telephone = Convert.ToString(column["Telephone"]),
                        Permissions = Convert.ToString(column["Permissions"]),
                    });
            }
            return businessData;
        }

        public List<BusinessModel> getBusinessByEmployeeID(string employeeID)
        {
            List<BusinessModel> businessData = new List<BusinessModel>();
            string query = "SELECT * FROM dbo.Employees JOIN Businesses ON Employees.BusinessID = Businesses.BusinessID " +
                            "WHERE Employees.UserID = " + employeeID;
            DataTable tableQueryResult = createTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(
                    new BusinessModel
                    {
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        Name = Convert.ToString(column["Name"]),
                        IDNumber = Convert.ToString(column["IDNumber"]),
                        Email = Convert.ToString(column["Email"]),
                        Telephone = Convert.ToString(column["Telephone"]),
                        Permissions = Convert.ToString(column["Permissions"]),
                    });
            }
            return businessData;
        }

        public bool insertNewBusiness(BusinessWithAddressModel newBusinessAddress)
        {
            var queryInsertBusiness = @"INSERT INTO Businesses ([Name], [IDNumber], [Email], [Telephone], [Permissions])" +
                                    "VALUES (@Name, @IDNumber, @Email, @Telephone, @Permissions);" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var commandInsertBusiness = new SqlCommand(queryInsertBusiness, _connection);
            commandInsertBusiness.Parameters.AddWithValue("@Name", newBusinessAddress.Name);
            commandInsertBusiness.Parameters.AddWithValue("@IDNumber", newBusinessAddress.IDNumber);
            commandInsertBusiness.Parameters.AddWithValue("@Email", newBusinessAddress.Email);
            commandInsertBusiness.Parameters.AddWithValue("@Telephone", newBusinessAddress.Telephone);
            commandInsertBusiness.Parameters.AddWithValue("@Permissions", newBusinessAddress.Permissions);

            var queryInsertBusinessAddress = @"INSERT INTO BusinessesAddresses" +
                                            "([BusinessID], [Province], [Canton], [District], [PostalCode], [OtherSigns])" +
                                            "VALUES(@BusinessID, @Province, @Canton, @District, @PostalCode, @OtherSigns);";
            var commandInsertBusinessAddress = new SqlCommand(queryInsertBusinessAddress, _connection);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Province", newBusinessAddress.Province);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Canton", newBusinessAddress.Canton);
            commandInsertBusinessAddress.Parameters.AddWithValue("@District", newBusinessAddress.District);
            commandInsertBusinessAddress.Parameters.AddWithValue("@PostalCode", newBusinessAddress.PostalCode);
            commandInsertBusinessAddress.Parameters.AddWithValue("@OtherSigns", newBusinessAddress.OtherSigns);

            _connection.Open();
            var transaction = _connection.BeginTransaction();
            commandInsertBusiness.Transaction = transaction;
            commandInsertBusinessAddress.Transaction = transaction;

            bool result;
            try
            {
                // Inserta el negocio y obtiene el BusinessID generado
                newBusinessAddress.BusinessID = Convert.ToInt32(commandInsertBusiness.ExecuteScalar());
                commandInsertBusinessAddress.Parameters.AddWithValue("@BusinessID", newBusinessAddress.BusinessID);

                result = commandInsertBusinessAddress.ExecuteNonQuery() >= 1;

                if (result)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                // Manejar la excepción (log o lo que sea necesario)
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
