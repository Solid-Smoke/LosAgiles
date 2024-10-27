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
            try
            {
                int? orderID = insertInOrders(orderData.ClientID, orderData.DeliveryAddress);
                if (orderID == null)
                    throw new Exception("Inserted OrderID is null");
                Console.WriteLine("Inserted orderID in transaction: " + orderID);
                bool insertedInOrderProducts = insertInOrderProducts((int)orderID, orderData.Products);
                bool InsertedInBusinessOrders = insertInBusinessOrders((int)orderID, orderData.Products);
                sqlConnection.Execute("COMMIT");
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                sqlConnection.Execute("ROLLBACK");
                sqlConnection.Close();
                throw new Exception(ex.ToString());
            }
        }

        private bool insertInBusinessOrders(int orderID, List<OrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { orderID, product.BusinessID });
            insertList = insertList.Distinct<object>().ToList();
            return sqlConnection.Execute("INSERT INTO BusinessOrders (OrderID, BusinessID) VALUES (@orderID, @BusinessID)", insertList) > 0;
        }

        private bool insertInOrderProducts(int orderID, List<OrderProductsModel> products)
        {
            var insertList = new List<object>();
            foreach (var product in products)
                insertList.Add(new { product.ProductID, orderID, product.Ammount });
            return sqlConnection.Execute("INSERT INTO OrderProducts (ProductId, OrderID, Amount) VALUES (@ProductId, @orderID, @Ammount)", insertList) > 0;
        }

        private int insertInOrders(int? clientID, ClientsAddress deliveryAddress)
        {

            return sqlConnection.QuerySingle<int>(
                "INSERT INTO Orders (CreatedDate, ClientID, DeliveryAddress)\r\n" +
                "OUTPUT INSERTED.OrderID\r\n" +
                "VALUES (getdate(), @clientID, @deliveryAddressID)",
                new { clientID, deliveryAddressID = deliveryAddress.AddressID }
            );
        }
    }
}
