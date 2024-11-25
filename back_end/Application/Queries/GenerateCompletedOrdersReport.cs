using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GenerateCompletedOrdersReport
    {
        private readonly CompletedOrderReport completedOrderReport;
        public GenerateCompletedOrdersReport(
            IReportHandler reportHandler)
        {
            completedOrderReport = new CompletedOrderReport(reportHandler);
        }

        public List<ReportCompletedOrderData> Execute(ReportBaseFilters baseFilters)
        {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.ClientID < 0)
                throw new ArgumentException("ClientID must be a non-negative value.", nameof(baseFilters));
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<ReportCompletedOrderData> reportData = completedOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0)
            {
                string orderIDs = completedOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = completedOrderReport.FetchOrderProducts(orderIDs);
                foreach (var completedOrder in reportData)
                {
                    var productsForCurrentOrder = orderProducts
                        .Where(product => product.OrderID == completedOrder.OrderID)
                        .ToList();

                    completedOrder.BusinessName = string.Join(", ", productsForCurrentOrder
                        .Select(product => product.BusinessName)
                        .Distinct());

                    completedOrder.Amount = productsForCurrentOrder.Sum(product => product.Amount);
                }
            }
            return reportData;
        }
    }
}
