using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Queries {
    public class GenerateAllPendingOrdersReport {
        private readonly AllPendingOrderReport pendingOrderReport;
        public GenerateAllPendingOrdersReport(
            IReportHandler reportHandler) {
            pendingOrderReport = new AllPendingOrderReport(reportHandler);
        }

        public List<AdminReportOrderData> Execute(ReportBaseFilters baseFilters) {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<AdminReportOrderData> reportData = pendingOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0) {
                string orderIDs = pendingOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = pendingOrderReport.FetchOrderProducts(orderIDs);
                foreach (var pendingOrder in reportData) {
                    var productsForCurrentOrder = orderProducts
                        .Where(product => product.OrderID == pendingOrder.OrderID)
                        .ToList();

                    pendingOrder.BusinessName = string.Join(", ", productsForCurrentOrder
                        .Select(product => product.BusinessName)
                        .Distinct());

                    pendingOrder.Amount = productsForCurrentOrder.Sum(product => product.Amount);
                }
            }
            return reportData;
        }
    }
}
