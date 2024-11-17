using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateCompletedOrdersReport
    {
        private readonly CompletedOrderReport completedOrderReport;
        public GenerateCompletedOrdersReport(
            IOrderHandler orderHandler)
        {
            completedOrderReport = new CompletedOrderReport(orderHandler);
        }

        public bool Execute(ReportBaseFilters baseFilters, out List<ReportCompletedOrderData> reportData)
        {
            List<ReportOrderProductData> orderProducts = new List<ReportOrderProductData>();
            reportData = new List<ReportCompletedOrderData>();
            string orderIDs = string.Empty;

            if (baseFilters == null || baseFilters.ClientID < 0)
            {
                Console.WriteLine("Error en los filtros base del reporte...");
                return false;
            }

            if (!completedOrderReport.GetOrderReport(baseFilters, out reportData, out orderIDs))
            {
                return false;
            }

            if (!completedOrderReport.GetOrderProducts(orderIDs, out orderProducts))
            {
                return false;
            }

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
            return true;
        }
    }
}
