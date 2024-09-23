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

        public List<Person> getPersons(int top)
        {
            using (sqlConnection)
            {
                sqlConnection.Open();
                return sqlConnection.Query<Person>($"select top {top} * from Person.Person").ToList();
            }
        }

    }
}