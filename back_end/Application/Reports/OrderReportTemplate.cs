using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;

public abstract class OrderReportTemplate<T> where T : ReportOrderData
{
    private readonly IOrderHandler _orderHandler;

    public OrderReportTemplate(IOrderHandler orderHandler)
    {
        _orderHandler = orderHandler;
    }

    public bool GetOrderReport(int clientID, out List<T> orderReport, out string orderIDs)
    {
        orderReport = new List<T>();
        orderIDs = string.Empty;

        try
        {
            string query = GenerateQuery(clientID);
            if (!_orderHandler.GetOrderReportData(query, out DataTable queryResultTable))
            {
                return false;
            }

            orderReport = ProcessQueryResult(queryResultTable, out orderIDs);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    protected abstract string GenerateQuery(int clientID);

    private List<T> ProcessQueryResult(DataTable queryResultTable, out string orderIDs)
    {
        var orderReport = new List<T>();
        var orderIdList = new List<int>();

        foreach (DataRow row in queryResultTable.Rows)
        {
            var orderData = MapOrderData(row);
            orderReport.Add(orderData);
            orderIdList.Add(orderData.OrderID);
        }

        orderIDs = string.Join(",", orderIdList);
        return orderReport;
    }

    protected abstract T MapOrderData(DataRow row);
}
