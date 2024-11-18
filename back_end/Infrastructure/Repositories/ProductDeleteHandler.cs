using back_end.Application.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Infrastructure.Repositories
{
    public class ProductDeleteHandler : IProductDeleteHandler
    {
        private readonly SqlConnection sqlConnection;

        public ProductDeleteHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<int> GetActiveOrderProductsIds(List<int> productIds)
        {
            List<string> activeOrderStatuses = new List<string> { "Pendiente", "Aprobada", "En envio" };
            return sqlConnection.Query<int>("SELECT ProductID FROM OrderProducts join Orders ON OrderProducts.OrderID = Orders.OrderID" +
                " WHERE (ProductID IN @productIds) AND (Orders.[Status] IN @activeOrderStatuses)",
                new { productIds, activeOrderStatuses }).ToList();
        }

        public List<int> GetInactiveOrderProductsIds(List<int> productIds)
        {
            List<string> inactiveOrderStatuses = new List<string> { "Rechazada", "Completada" };
            return sqlConnection.Query<int>("SELECT ProductID FROM OrderProducts join Orders ON OrderProducts.OrderID = Orders.OrderID" +
                " WHERE (ProductID IN @productIds) AND (Orders.[Status] IN @inactiveOrderStatuses)",
                new { productIds, inactiveOrderStatuses }).ToList();
        }

        public List<int> GetInShoppingCartProductsIds(List<int> productIds)
        {
            return sqlConnection.Query<int>("SELECT ProductID FROM ShoppingCarts WHERE ProductID IN @productIds",
                new { productIds }).ToList();
        }

        public bool DeleteFromShoppingCarts(List<int> productIds)
        {
            int rowsAffected = sqlConnection.Execute("DELETE FROM ShoppingCarts WHERE ProductID IN @productIds", new { productIds });
            return rowsAffected == productIds.Count;
        }

        public bool HardDelete(List<int> productIds)
        {
            int rowsAffected = sqlConnection.Execute("DELETE FROM Products WHERE ProductID IN @productIds", new { productIds });
            return rowsAffected == productIds.Count;
        }

        public bool SoftDelete(List<int> productIds)
        {
            int rowsAffected = 0;
            foreach (int productId in productIds)
            {
                rowsAffected += sqlConnection.Execute("UPDATE Products SET Products.IsDeleted = 1" +
                    " WHERE ProductId = @productId AND IsDeleted = 0", new { productId });
            }
            return rowsAffected == productIds.Count;
        }

        public void OpenSqlConnection()
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void CloseSqlConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public void BeginSerializableTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Execute("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; BEGIN TRANSACTION;");
            }
        }

        public void RollbackTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Execute("ROLLBACK");
            }
        }

        public void CommitTransaction()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Execute("COMMIT");
            }
        }
    }
}
