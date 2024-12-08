﻿using back_end.Application.interfaces;
using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Infrastructure.Repositories
{
    public class BusinessHandler : IBusinessHandler
    {
        private SqlConnection _connection;
        private string? _routeConnection;

        public BusinessHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("ClientsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        private DataTable createTableResult(SqlCommand command)
        {
            SqlDataAdapter tableAdapter = new SqlDataAdapter(command);
            DataTable resultTable = new DataTable();
            _connection.Open();
            tableAdapter.Fill(resultTable);
            _connection.Close();
            return resultTable;
        }

        public List<BusinessModel> getAllBusiness()
        {
            List<BusinessModel> businessData = new List<BusinessModel>();
            string query = "SELECT * FROM [dbo].[Businesses] WHERE IsDeleted = 0";
            SqlCommand command = new SqlCommand(query, _connection);
            DataTable tableQueryResult = createTableResult(command);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(new BusinessModel
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
            string query = @"
                SELECT * 
                FROM [dbo].[Employees] 
                JOIN [dbo].[Businesses] ON [Employees].[BusinessID] = [Businesses].[BusinessID] 
                WHERE [Employees].[UserID] = @EmployeeID AND IsDeleted = 0";
            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);
            DataTable tableQueryResult = createTableResult(command);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(new BusinessModel
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
            string query = "SELECT * FROM [dbo].[BusinessesAddresses] WHERE [BusinessID] = @BusinessID";
            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@BusinessID", businessID);
            DataTable tableQueryResult = createTableResult(command);
            foreach (DataRow column in tableQueryResult.Rows)
            {
                businessData.Add(new BusinessAddressModel
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
            var queryInsertBusiness = "INSERT INTO [dbo].[Businesses] ([Name], [IDNumber], [Email], [Telephone], [Permissions])" +
                                      "VALUES (@Name, @IDNumber, @Email, @Telephone, @Permissions);" +
                                      "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var commandInsertBusiness = new SqlCommand(queryInsertBusiness, _connection);
            commandInsertBusiness.Parameters.AddWithValue("@Name", newBusiness.Name);
            commandInsertBusiness.Parameters.AddWithValue("@IDNumber", newBusiness.IDNumber);
            commandInsertBusiness.Parameters.AddWithValue("@Email", newBusiness.Email);
            commandInsertBusiness.Parameters.AddWithValue("@Telephone", newBusiness.Telephone);
            commandInsertBusiness.Parameters.AddWithValue("@Permissions", newBusiness.Permissions);

            var queryInsertBusinessAddress = "INSERT INTO [dbo].[BusinessesAddresses] " +
                                             "([BusinessID], [Province], [Canton], [District], [PostalCode], [OtherSigns]) " +
                                             "VALUES(@BusinessID, @Province, @Canton, @District, @PostalCode, @OtherSigns);";
            var commandInsertBusinessAddress = new SqlCommand(queryInsertBusinessAddress, _connection);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Province", newBusiness.Province);
            commandInsertBusinessAddress.Parameters.AddWithValue("@Canton", newBusiness.Canton);
            commandInsertBusinessAddress.Parameters.AddWithValue("@District", newBusiness.District);
            commandInsertBusinessAddress.Parameters.AddWithValue("@PostalCode", newBusiness.PostalCode);
            commandInsertBusinessAddress.Parameters.AddWithValue("@OtherSigns", newBusiness.OtherSigns);

            var queryInsertEmployee = "INSERT INTO [dbo].[Employees] ([BusinessID], [UserID]) VALUES(@BusinessID, @UserID);";
            var commandInsertEmployee = new SqlCommand(queryInsertEmployee, _connection);
            commandInsertEmployee.Parameters.AddWithValue("@UserID", newBusiness.UserID);

            _connection.Open();
            var transaction = _connection.BeginTransaction();
            commandInsertBusiness.Transaction = transaction;
            commandInsertBusinessAddress.Transaction = transaction;
            commandInsertEmployee.Transaction = transaction;
            bool result;

            try
            {
                newBusiness.BusinessID = Convert.ToInt32(commandInsertBusiness.ExecuteScalar());
                commandInsertBusinessAddress.Parameters.AddWithValue("@BusinessID", newBusiness.BusinessID);
                commandInsertEmployee.Parameters.AddWithValue("@BusinessID", newBusiness.BusinessID);

                var businessInserted = commandInsertBusinessAddress.ExecuteNonQuery() >= 1;
                var employeeInserted = commandInsertEmployee.ExecuteNonQuery() >= 1;

                result = businessInserted && employeeInserted;

                if (result) transaction.Commit();
                else transaction.Rollback();

                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
        public BusinessModel getBusinessByID(int businessID)
        {
            string query = "SELECT * FROM [dbo].[Businesses] WHERE BusinessID = @BusinessID";
            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@BusinessID", businessID);

            DataTable tableQueryResult = createTableResult(command);

            if (tableQueryResult.Rows.Count == 0)
            {
                return null;
            }

            var column = tableQueryResult.Rows[0];
            return new BusinessModel
            {
                BusinessID = Convert.ToInt32(column["BusinessID"]),
                Name = Convert.ToString(column["Name"]),
                IDNumber = Convert.ToString(column["IDNumber"]),
                Email = Convert.ToString(column["Email"]),
                Telephone = Convert.ToString(column["Telephone"]),
                Permissions = Convert.ToString(column["Permissions"]),
            };
        }

        public List<MonthlyRevenueModel> GetMonthlyRevenueByBusinessID(int businessID)
        {
            string query = @"
                SELECT 
                    MONTH(o.CreatedDate) AS Month, 
                    SUM(op.Amount * p.Price) AS Total
                FROM [dbo].[Orders] o
                JOIN [dbo].[OrderProducts] op ON o.OrderID = op.OrderID
                JOIN [dbo].[Products] p ON op.ProductID = p.ProductID
                WHERE p.BusinessID = @BusinessID AND Status = 'Completada'
                GROUP BY MONTH(o.CreatedDate)
                ORDER BY MONTH(o.CreatedDate);";

            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@BusinessID", businessID);

            DataTable tableQueryResult = createTableResult(command);

            List<MonthlyRevenueModel> revenues = new List<MonthlyRevenueModel>();
            foreach (DataRow row in tableQueryResult.Rows)
            {
                revenues.Add(new MonthlyRevenueModel
                {
                    Month = Convert.ToInt32(row["Month"]),
                    Total = Convert.ToDecimal(row["Total"]),
                });
            }

            return revenues;
        }

        public List<OrderModel> GetOrdersInProgressByBusinessID(int businessID)
        {
            string query = @"
                SELECT TOP 10
                    o.OrderID,
                    o.Status,
                    o.CreatedDate,
                    o.DeliveryDate,
                    o.ClientID,
                    o.DeliveryAddress,
                    SUM(op.Amount * p.Price) AS TotalCost
                FROM [dbo].[Orders] o
                JOIN [dbo].[OrderProducts] op ON o.OrderID = op.OrderID
                JOIN [dbo].[Products] p ON op.ProductID = p.ProductID
                WHERE p.BusinessID = @BusinessID AND o.Status <> 'Completada' AND o.Status <> 'Rechazada'
                GROUP BY 
                    o.OrderID,
                    o.Status,
                    o.CreatedDate,
                    o.DeliveryDate,
                    o.ClientID,
                    o.DeliveryAddress";

            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@BusinessID", businessID);

            DataTable tableQueryResult = createTableResult(command);

            List<OrderModel> orders = new List<OrderModel>();
            foreach (DataRow row in tableQueryResult.Rows)
            {
                orders.Add(new OrderModel
                {
                    OrderID = Convert.ToInt32(row["OrderID"]),
                    Status = Convert.ToString(row["Status"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    DeliveryDate = row["DeliveryDate"] == DBNull.Value ? null : Convert.ToDateTime(row["DeliveryDate"]),
                    ClientID = row["ClientID"] == DBNull.Value ? null : Convert.ToInt32(row["ClientID"]),
                    DeliveryAddress = row["DeliveryAddress"] == DBNull.Value ? null : Convert.ToInt32(row["DeliveryAddress"]),
                    TotalAmount = Convert.ToDecimal(row["TotalCost"])
                });
            }

            return orders;
        }
    }
}
