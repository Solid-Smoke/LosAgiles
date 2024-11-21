using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Queries {
    public class GenerateAllCancelledOrdersReport {
        private readonly CancelledOrderReport cancelledOrderReport;
        public GenerateAllCancelledOrdersReport(
            IReportHandler reportHandler) {
            cancelledOrderReport = new CancelledOrderReport(reportHandler);
        }

        public List<AdminReportOrderData> Execute(ReportBaseFilters baseFilters) {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<AdminReportOrderData> reportData = cancelledOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0) {
                string orderIDs = cancelledOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = cancelledOrderReport.FetchOrderProducts(orderIDs);
                foreach (var cancelledOrder in reportData) {
                    var productsForCurrentOrder = orderProducts
                        .Where(product => product.OrderID == cancelledOrder.OrderID)
                        .ToList();

                    cancelledOrder.BusinessName = string.Join(", ", productsForCurrentOrder
                        .Select(product => product.BusinessName)
                        .Distinct());

                    cancelledOrder.Amount = productsForCurrentOrder.Sum(product => product.Amount);
                }
            }
            return reportData;
        }
    }
}
