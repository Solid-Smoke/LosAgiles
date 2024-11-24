using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateAllCancelledOrderReportPDF
    {
        private readonly OrderReportTemplate<AdminReportOrderData> _cancelledOrderReport;
        public GenerateAllCancelledOrderReportPDF(
            OrderReportTemplate<AdminReportOrderData> reportHandler)
        {
            _cancelledOrderReport = reportHandler;
        }

        public byte[] Execute(ReportBaseFilters baseFilters)
        {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<AdminReportOrderData> reportData = _cancelledOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0)
            {
                string orderIDs = _cancelledOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = _cancelledOrderReport.FetchOrderProducts(orderIDs);
                foreach (var cancelledOrder in reportData)
                {
                    var productsForCurrentOrder = orderProducts
                        .Where(product => product.OrderID == cancelledOrder.OrderID)
                        .ToList();

                    cancelledOrder.BusinessName = string.Join(", ", productsForCurrentOrder
                        .Select(product => product.BusinessName)
                        .Distinct());

                    cancelledOrder.Amount = productsForCurrentOrder.Sum(product => product.Amount);
                }
                return _cancelledOrderReport.GeneratePdf(reportData, baseFilters);
            }
            return new byte[0];
        }
    }
}
