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

        private int query(string query)
        {
            sqlConnection.Open();
            int rowsAffected = sqlConnection.Execute(query);
            sqlConnection.Close();
            return rowsAffected;
        }

        public int storeClientAddress(ClientsAddress address)
        {
            string query = $@"INSERT INTO ClientsAddresses
                            (UserID,
                            Province,
                            Canton,
                            District,
                            PostalCode,
                            OtherSigns)
                            VALUES (
                            {address.UserID},
                            '{address.Province}',
                            '{address.Canton}',
                            '{address.District}',
                            {address.PostalCode},
                            '{address.OtherSigns}')";
            return this.query(query);
        }
    }
}