using System.Data.SqlClient;
using back_end.Application.Interfaces;
using back_end.Domain;
using Dapper;

namespace back_end.Infrastructure.Repositories
{
    public class OrdersHandler : IOrdersHandler
    {
        private readonly SqlConnection sqlConnection;

        public OrdersHandler(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool createOrder(OrderModel orderData)
        {
            sqlConnection.Open();
            sqlConnection.Execute("BEGIN TRAN");
            Guid orderID = sqlConnection.Query<Guid>("SELECT newid()").ToList<Guid>()[0];
            bool insertedInOrders = insertInOrders(orderData.ClientID, orderID, orderData.DeliveryAddress);
            bool insertedInOrderProducts = insertInOrderProducts(orderID, orderData.Products);
            bool InsertedInBusinessOrders = insertInBusinessOrders(orderID, orderData.Products);
            if (insertedInOrders && insertedInOrderProducts && InsertedInBusinessOrders)
            {
                sqlConnection.Execute("COMMIT");
                sqlConnection.Close();
                return true;
            }
            else
            {
                sqlConnection.Execute("ROLLBACK");
                sqlConnection.Close();
                return false;
            }
        }

        private bool insertInBusinessOrders(Guid orderID, List<OrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { orderID, product.BusinessID });
            return sqlConnection.Execute("INSERT INTO BusinessOrders VALUES (@orderID, @businessID)", insertList) > 0;
        }

        private bool insertInOrderProducts(Guid orderID, List<OrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { product.ProductID, orderID, product.Ammount });
            return sqlConnection.Execute("INSERT INTO OrderProducts VALUES (@productId, @orderID, @ammount)", insertList) > 0;
        }

        private bool insertInOrders(int clientID, Guid orderID, ClientsAddress deliveryAddress)
        {
            return sqlConnection.Execute(
                "INSERT INTO Orders (OrderID, [Status], CreatedDate, ClientID, DeliveryAddress)\r\n" +
                "VALUES (@orderID, 'Submitted', getdate(), @clientID, @deliveryAddressID)",
                new { orderID, clientID, deliveryAddressID = deliveryAddress.AddressID }
            ) > 0;
        }
    }
}
