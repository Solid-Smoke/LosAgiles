using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateAllPendingOrderReportPDF
    {
        private readonly OrderReportTemplate<AdminReportOrderData> _pendingOrderReport;
        public GenerateAllPendingOrderReportPDF(
            OrderReportTemplate<AdminReportOrderData> reportHandler)
        {
            _pendingOrderReport = reportHandler;
        }

        public byte[] Execute(ReportBaseFilters baseFilters)
        {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<AdminReportOrderData> reportData = _pendingOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0)
            {
                string orderIDs = _pendingOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = _pendingOrderReport.FetchOrderProducts(orderIDs);
                foreach (var pendingOrder in reportData)
                {
                    var productsForCurrentOrder = orderProducts
                        .Where(product => product.OrderID == pendingOrder.OrderID)
                        .ToList();

                    pendingOrder.BusinessName = string.Join(", ", productsForCurrentOrder
                        .Select(product => product.BusinessName)
                        .Distinct());

                    pendingOrder.Amount = productsForCurrentOrder.Sum(product => product.Amount);
                }
                return _pendingOrderReport.GeneratePdf(reportData, baseFilters);
            }
            return new byte[0];
        }
    }
}
