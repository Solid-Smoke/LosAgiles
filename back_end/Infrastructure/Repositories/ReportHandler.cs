using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Infrastructure.Repositories
{
    public class ReportHandler : IReportHandler
    {
        private readonly SqlConnection sqlConnection;

        public ReportHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public DataTable FetchReportOrderData(string query, ReportBaseFilters baseFilters)
        {
            DataTable queryResultTable = new DataTable();
            try
            {
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ClientID", baseFilters.ClientID);
                    sqlCommand.Parameters.AddWithValue("@StartDate", baseFilters.StartDate);
                    sqlCommand.Parameters.AddWithValue("@EndDate", baseFilters.EndDate);
                    sqlConnection.Open();
                    using (var sqlAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlAdapter.Fill(queryResultTable);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException("An error occurred while fetching completed orders data.", sqlEx);
            }
            finally
            {
                sqlConnection.Close();
            }
            return queryResultTable;
        }
        public DataTable FetchOrderProductsData(string orderIDs)
        {
            DataTable queryResultTable = new DataTable();
            try
            {
                string query = $@"SELECT 
                                    [op].OrderID, [op].Amount, [b].[Name] as BusinessName
                                  FROM 
                                    [OrderProducts] [op]
                                  INNER JOIN 
                                    [Products] [p] ON [p].[ProductID] = [op].[ProductID]
                                  INNER JOIN 
                                    [Businesses] [b] ON [b].[BusinessID] = [p].[BusinessID]
                                  WHERE
                                    [op].OrderID IN ({orderIDs})
                ";

                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlAdapter.Fill(queryResultTable);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException("An error occurred while fetching order products data.", sqlEx);
            }
            finally
            {
                sqlConnection.Close();
            }
            return queryResultTable;
        }
        public DataTable FetchEarningsReport(int year, int? businessId = null)
        {
            DataTable queryResultTable = new DataTable();
            try
            {
                string query = @"SELECT 
                                    [BusinessName], 
                                    [Month], 
                                    [TotalPurchase], 
                                    [DeliveryCost], 
                                    [TotalCost]
                                 FROM 
                                    [dbo].[GetBusinessEarningsReportByMonthAndYear](@Year, @BusinessID)";

                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Year", year);

                    if (businessId.HasValue)
                    {
                        sqlCommand.Parameters.AddWithValue("@BusinessID", businessId.Value);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@BusinessID", DBNull.Value);
                    }

                    sqlConnection.Open();

                    using (var sqlAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlAdapter.Fill(queryResultTable);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new InvalidOperationException("An error occurred while fetching the completed orders report.", sqlEx);
            }
            finally
            {
                sqlConnection.Close();
            }
            return queryResultTable;
        }
    }
}
