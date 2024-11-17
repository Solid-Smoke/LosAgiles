using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;

public abstract class OrderReportTemplate<T> where T : ReportOrderData
{
    protected readonly IOrderHandler _orderHandler;

    public OrderReportTemplate(IOrderHandler orderHandler)
    {
        _orderHandler = orderHandler;
    }

    public bool GetOrderReport(ReportBaseFilters baseFilters,
        out List<T> orderReport, out string orderIDs)
    {
        orderReport = new List<T>();
        orderIDs = string.Empty;
        DataTable queryResultTable;

        if (baseFilters == null || baseFilters.ClientID < 0)
        {
            Console.WriteLine("Error en el manejo de los filtros base del reporte...");
            return false;
        }

        try
        {
            if (!ExecuteReportQuery(baseFilters, out queryResultTable))
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

    public bool GetOrderProducts(string orderIDs, out List<ReportOrderProductData> orderProducts)
    {
        orderProducts = new List<ReportOrderProductData>();
        DataTable queryResultTable;

        try
        {
            if (!_orderHandler.GetOrderProductsData(orderIDs, out queryResultTable))
            {
                return false;
            }

            foreach (DataRow row in queryResultTable.Rows)
            {
                var orderProductData = MapOrderProductData(row);
                orderProducts.Add(orderProductData);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order products: {ex.Message}");
            return false;
        }
    }

    private ReportOrderProductData MapOrderProductData(DataRow row)
    {
        return new ReportOrderProductData
        {
            OrderID = Convert.ToInt32(row["OrderID"]),
            Amount = Convert.ToInt32(row["Amount"]),
            BusinessName = row["BusinessName"] != DBNull.Value ? Convert.ToString(row["BusinessName"]) : ""
        };
    }

    protected abstract bool ExecuteReportQuery(ReportBaseFilters baseFilters, out DataTable queryResultTable);
    protected abstract T MapOrderData(DataRow row);
}
