using back_end.Application.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Infrastructure.Repositories
{
    public class BusinessDeleteHandler : IBusinessDeleteHandler
    {
        private readonly SqlConnection sqlConnection;

        public BusinessDeleteHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<int> GetBusinessProductsIds(int businessId)
        {
            return sqlConnection.Query<int>("SELECT ProductID FROM Products WHERE BusinessID = @businessId", new { businessId }).ToList();
        }

        public void OpenSqlConnection()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void CloseSqlConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }

        public void BeginTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Execute("BEGIN TRANSACTION;");
        }

        public void RollbackTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Execute("ROLLBACK");
        }

        public void CommitTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Execute("COMMIT");
        }

        public void DeleteEmployees(int businessId)
        {
            sqlConnection.Execute("DELETE FROM Employees WHERE BusinessID = @businessId", new { businessId });
        }

        public void DeleteBusinessAddress(int businessId)
        {
            sqlConnection.Execute("DELETE FROM BusinessAddresses WHERE BusinessID = @businessId", new { businessId });
        }

        public bool CheckIfAProductWasSoftDeleted(int businessId)
        {
            return sqlConnection.QueryFirstOrDefault<bool>("SELECT IsDeleted FROM Products WHERE BusinessID = @businessId", new { businessId });
        }

        public void SoftDeleteBusiness(int businessId)
        {
            sqlConnection.Execute("UPDATE Businesses SET IsDeleted = 1 WHERE BusinessID = @businessId", new { businessId });
        }

        public void HardDeleteBusiness(int businessId)
        {
            sqlConnection.Execute("DELETE FROM Businesses WHERE BusinessID = @businessId", new { businessId });
        }

        public void SetTransactionIsolationLevel(string isolationLevel)
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Execute($"SET TRANSACTION ISOLATION LEVEL {isolationLevel};");
        }
    }
}
