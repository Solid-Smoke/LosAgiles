using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public abstract class OrderReportTemplate<T> where T : ReportOrderData
{
    protected readonly IReportHandler _reportHandler;

    public OrderReportTemplate(IReportHandler reportHandler)
    {
        _reportHandler = reportHandler;
    }

    public List<T> FetchReportOrders(ReportBaseFilters baseFilters)
    {
        if (baseFilters == null)
            throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
        if (baseFilters.ClientID < 0)
            throw new ArgumentException("ClientID must be a non-negative value.", nameof(baseFilters));
        if (baseFilters.StartDate > baseFilters.EndDate)
            throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));

        try
        {
            DataTable queryResultTable = ExecuteReportQuery(baseFilters);
            return ProcessQueryResult(queryResultTable);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while fetching orders (FetchReportOrders).", ex);
        }
    }

    public List<ReportOrderProductData> FetchOrderProducts(string orderIDs)
    {
        List<ReportOrderProductData> orderProducts = new List<ReportOrderProductData>();
        try
        {
            DataTable queryResultTable = _reportHandler.FetchOrderProductsData(orderIDs);

            foreach (DataRow row in queryResultTable.Rows)
            {
                var orderProductData = MapOrderProductData(row);
                orderProducts.Add(orderProductData);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while fetching order products (FetchOrderProducts).", ex);
        }
        return orderProducts;
    }
    public string GetOrderIDsFromReport(IEnumerable<T> reportData)
    {
        if (reportData == null || reportData.Count() < 0)
            throw new ArgumentException("The report cannot be null or empty.", nameof(reportData));

        var resultBuilder = new StringBuilder();
        foreach (var item in reportData)
        {
            if (resultBuilder.Length > 0)
                resultBuilder.Append(",");
            resultBuilder.Append(item.OrderID);
        }
        return resultBuilder.ToString();
    }

    private List<T> ProcessQueryResult(DataTable queryResultTable)
    {
        var orderReport = new List<T>();

        foreach (DataRow row in queryResultTable.Rows)
        {
            var orderData = MapOrderData(row);
            orderReport.Add(orderData);
        }
        return orderReport;
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

    protected abstract DataTable ExecuteReportQuery(ReportBaseFilters baseFilters);
    protected abstract T MapOrderData(DataRow row);
}
