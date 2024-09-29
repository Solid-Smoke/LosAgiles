using back_end.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
namespace back_end.Handlers
{
    public class ApplicationHandler
    {
        private SqlConnection sqlConnection;
        public ApplicationHandler()
        {
            var builder = WebApplication.CreateBuilder();
            var connectionRoute =
            builder.Configuration.GetConnectionString("ApplicationDBContext");
            sqlConnection = new SqlConnection(connectionRoute);
        }

        public List<ClientsAddresses> getAllClientAddresses(int clientID)
        {
            string query = $@"SELECT * FROM ClientsAddresses
                            WHERE UserID = {clientID}";
            sqlConnection.Open();
            var addresses = sqlConnection.Query<ClientsAddresses>(query)
                .ToList();
            sqlConnection.Close();
            return addresses;
        }
    }
}