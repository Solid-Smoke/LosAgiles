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

        public List<Users> getAllUsersData(int offset, int maxRows)
        {
            string query = $@"SELECT Clients.UserID,
                            Clients.UserName,
                            Clients.Email,
                            Clients.AccountState,
                            Clients.[Name],
                            Clients.LastNames,
                            EmployeesBusinessesNames.BusinessName
                            FROM Clients LEFT JOIN (
                                select Businesses.Name as BusinessName,
                                Employees.UserID
                                from Employees inner join Businesses
                                on Employees.BusinessID = Businesses.BusinessID
                            ) AS EmployeesBusinessesNames
                            ON Clients.UserID = EmployeesBusinessesNames.UserID
                               ORDER BY Clients.UserID
                               OFFSET {offset} ROWS
                               FETCH NEXT {maxRows} ROWS ONLY;";
            sqlConnection.Open();
            List<Users> result = sqlConnection.Query<Users>(query)
                                     .ToList();
            sqlConnection.Close();
            return result;
            
        }

        public int getUserCount()
        {
            string query = "SELECT COUNT(*) FROM Clients";
            sqlConnection.Open();
            int result = sqlConnection.Query<int>(query).ToList()[0];
            sqlConnection.Close();
            return result;
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

        public bool verifySuperUserId(int realId)
        {
            string query = $@"SELECT SuperUserID FROM SuperUsers
                            WHERE SuperUserID = {realId}";

            bool isSuperUser = false;
            sqlConnection.Open();
            List<SuperUsers> queryResult = sqlConnection.Query<SuperUsers>(query).ToList();
            if (queryResult.Count > 0)
            {
                isSuperUser = true;
            }
            else
            {
                isSuperUser = false;
            }
            sqlConnection.Close();
            return isSuperUser;
        }
        public List<ClientsAddresses> getAllClientAddresses(int userId)
        {
            sqlConnection.Open();
            string query = $@"SELECT * FROM ClientsAddresses
                       WHERE UserID = {userId}";
            var addresses = sqlConnection.Query<ClientsAddresses>(query)
                .ToList();
            sqlConnection.Close();
            return addresses;
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