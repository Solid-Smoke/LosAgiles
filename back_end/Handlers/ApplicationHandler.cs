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

        private int query(string query) {
            sqlConnection.Open();
            int rowsAffected = sqlConnection.Execute(query);
            sqlConnection.Close();
            return rowsAffected;
        }

        public int storeUsers(Users user) {
            string query = $@"INSERT INTO Clients
                            (Name,
                            LastNames,
                            UserName,
                            Email,
                            BirthDate,
                            UserPassword)
                            VALUES (
                            '{user.Name}',
                            '{user.lastNames}',
                            '{user.userName}',
                            '{user.Email}',
                            '{user.BirthDate}',
                            '{user.UserPassword}')";
            return this.query(query);
        }
    }
}