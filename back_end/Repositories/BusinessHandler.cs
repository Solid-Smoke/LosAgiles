﻿using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Repositories
{
    public class BusinessHandler
    {
        private SqlConnection _connection;
        private string? _routeConnection;

        public BusinessHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("ClientsContext");
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
        public List<BusinessAddressModel> getBusinessAddressByBusinessID(string businessID)
        {
            List<BusinessAddressModel> businessData = new List<BusinessAddressModel>();
            string query = "SELECT * FROM BusinessesAddresses WHERE BusinessID = " + businessID;
            DataTable tableQueryResult = createTableResult(query);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(
                    new BusinessAddressModel
                    {
                        BusinessID = Convert.ToInt32(column["BusinessID"]),
                        Province = Convert.ToString(column["Province"]),
                        Canton = Convert.ToString(column["Canton"]),
                        District = Convert.ToString(column["District"]),
                        PostalCode = Convert.ToString(column["PostalCode"]),
                        OtherSigns = Convert.ToString(column["OtherSigns"]),
                    });
            }
            return businessData;
        }

        public bool insertNewBusiness(CompleteBusinessModel newBusiness)
        {
            var queryInsertBusiness = @"INSERT INTO Businesses ([Name], [IDNumber], [Email], [Telephone], [Permissions])" +
                                    "VALUES (@Name, @IDNumber, @Email, @Telephone, @Permissions);" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var commandInsertBusiness = new SqlCommand(queryInsertBusiness, _connection);
            commandInsertBusiness.Parameters.AddWithValue("@Name", newBusiness.Name);
            commandInsertBusiness.Parameters.AddWithValue("@IDNumber", newBusiness.IDNumber);
            commandInsertBusiness.Parameters.AddWithValue("@Email", newBusiness.Email);
            commandInsertBusiness.Parameters.AddWithValue("@Telephone", newBusiness.Telephone);
            commandInsertBusiness.Parameters.AddWithValue("@Permissions", newBusiness.Permissions);

            var queryInsertBusinessAddress = @"INSERT INTO BusinessesAddresses" +
                                            "([BusinessID], [Province], [Canton], [District], [PostalCode], [OtherSigns])" +
                                            "VALUES(@BusinessID, @Province, @Canton, @District, @PostalCode, @OtherSigns);";
            var commandInsertBusinessAddress = new SqlCommand(queryInsertBusinessAddress, _connection);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Province", newBusiness.Province);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Canton", newBusiness.Canton);
            commandInsertBusinessAddress.Parameters.AddWithValue("@District", newBusiness.District);
            commandInsertBusinessAddress.Parameters.AddWithValue("@PostalCode", newBusiness.PostalCode);
            commandInsertBusinessAddress.Parameters.AddWithValue("@OtherSigns", newBusiness.OtherSigns);

            var queryInsertEmployee = @"INSERT INTO Employees (BusinessID, UserID)" +
                                       "VALUES(@BusinessID, @UserID);";
            var commandInsertEmployee = new SqlCommand(queryInsertEmployee, _connection);
            commandInsertEmployee.Parameters.AddWithValue("@UserID", newBusiness.UserID); // clientId será el ID del cliente (UserID)

            _connection.Open();
            var transaction = _connection.BeginTransaction();
            commandInsertBusiness.Transaction = transaction;
            commandInsertBusinessAddress.Transaction = transaction;
            commandInsertEmployee.Transaction = transaction;
            bool result;
            try
            {
                // Inserta el negocio y obtiene el BusinessID generado
                newBusiness.BusinessID = Convert.ToInt32(commandInsertBusiness.ExecuteScalar());
                commandInsertBusinessAddress.Parameters.AddWithValue("@BusinessID", newBusiness.BusinessID);
                commandInsertEmployee.Parameters.AddWithValue("@BusinessID", newBusiness.BusinessID);

                // Ejecuta las inserciones de negocio, dirección y empleado
                var businessInserted = commandInsertBusinessAddress.ExecuteNonQuery() >= 1;
                var employeeInserted = commandInsertEmployee.ExecuteNonQuery() >= 1;

                result = businessInserted && employeeInserted;

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