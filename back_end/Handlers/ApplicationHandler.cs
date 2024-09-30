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

        public List<ClientsAddresses> getAllClientAddresses(string userName)
        {
            sqlConnection.Open();
            string query = "SELECT UserID FROM Clients " +
                $"WHERE UserName = '{userName}'";
            List<int> result = sqlConnection.Query<int>(query).ToList();
            int userId = -1;
            if (result.Count > 0)
            {
                userId = result[0];
            } else
            {
                throw new Exception(
                    "getAllClientAddresses: Username doesnt exist");
            }

            query = $@"SELECT * FROM ClientsAddresses
                       WHERE UserID = {userId}";
            var addresses = sqlConnection.Query<ClientsAddresses>(query)
                .ToList();
            sqlConnection.Close();
            return addresses;
        }
    }
}