using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data.SqlClient;
using System.Collections.Generic;

public class AdminHandler : IAdminHandler
{
    private readonly SqlConnection _sqlConnection;

    public AdminHandler(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    public List<MonthlyDataModel> GetMonthlyRevenue()
    {
        var result = new List<MonthlyDataModel>();
        var query = "SELECT MONTH(CreatedDate) AS Month, SUM(TotalCost) AS Total FROM Orders WHERE STATUS = 'Completada' GROUP BY MONTH(CreatedDate) ORDER BY MONTH(CreatedDate) ";

        using (var command = new SqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new MonthlyDataModel
                    {
                        Month = reader["Month"].ToString(),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                    });
                }
            }
            _sqlConnection.Close();
        }

        return result;
    }

    public List<MonthlyDataModel> GetMonthlyShippingExpense()
    {
        var result = new List<MonthlyDataModel>();
        var query = "SELECT MONTH(DeliveryDate) AS Month, SUM(DeliveryCost) AS Total FROM Orders WHERE STATUS = 'Completada' GROUP BY MONTH(DeliveryDate) ORDER BY MONTH(DeliveryDate)";

        using (var command = new SqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new MonthlyDataModel
                    {
                        Month = reader["Month"].ToString(),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                    });
                }
            }
            _sqlConnection.Close();
        }

        return result;
    }

    public List<OrderModel> GetOrdersInProgress()
    {
        var result = new List<OrderModel>();
        var query = "SELECT TOP 10 OrderID, Status, TotalCost FROM Orders WHERE Status != 'Completada' AND Status != 'Rechazada' ORDER BY CreatedDate ASC";

        using (var command = new SqlCommand(query, _sqlConnection))
        {
            _sqlConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new OrderModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        Status = reader["Status"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalCost"])
                    });
                }
            }
            _sqlConnection.Close();
        }

        return result;
    }
}
