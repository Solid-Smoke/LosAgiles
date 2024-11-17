using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateCompletedOrdersReport
    {
        private readonly CompletedOrderReport _completedOrderReport;
        public GenerateCompletedOrdersReport(
            IOrderHandler orderHandler)
        {
            _completedOrderReport = new CompletedOrderReport(orderHandler);
        }

        public bool Execute(ReportBaseFilters baseFilters, out List<ReportCompletedOrderData> reportData, out string orderIDs)
        {
            reportData = new List<ReportCompletedOrderData>();
            orderIDs = string.Empty;

            if (baseFilters == null || baseFilters.ClientID < 0)
            {
                Console.WriteLine("Error en los filtros base del reporte...");
                return false;
            }

            try
            {
                return _completedOrderReport.GetOrderReport(baseFilters, out reportData, out orderIDs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar el comando: {ex.Message}");
                return false;
            }
        }
    }
}
