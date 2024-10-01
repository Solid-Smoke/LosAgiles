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

        public int authSuperUser(string userName, string passwordHash)
        {
            string query = $@"SELECT SuperUserID FROM SuperUsers
                            WHERE Username = '{userName}'
                            AND [Password] = '{passwordHash}'";
            int superUserId;
            sqlConnection.Open();
            List<SuperUsers> queryResult = sqlConnection.Query<SuperUsers>(query).ToList();
            if (queryResult.Count > 0)
            {
                superUserId = queryResult[0].SuperUserID;
            } else
            {
                throw new Exception("authSuperUser: Bad user or password");
            }
            sqlConnection.Close();
            return superUserId;
        }
    }
}