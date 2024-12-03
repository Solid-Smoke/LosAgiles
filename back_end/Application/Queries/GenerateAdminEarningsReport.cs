using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GenerateAdminEarningsReport
    {
        private readonly AdminEarningsReport _earningsReport;

        public GenerateAdminEarningsReport(IReportHandler reportHandler)
        {
            _earningsReport = new AdminEarningsReport(reportHandler);
        }

        public List<ReportEarningsData> Execute(ReportEarningsFilters filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters), "Filters cannot be null.");
            if (filters.Year < 0)
                throw new ArgumentException("Year must be a non-negative value.", nameof(filters));

            List<ReportEarningsData> reportData = _earningsReport.FetchEarningsReport(filters);

            if (reportData.Count > 0)
            {
                decimal totalPurchaseGlobal = 0;
                decimal totalDeliveryCostGlobal = 0;
                decimal totalCostGlobal = 0;

                foreach (var data in reportData)
                {
                    totalPurchaseGlobal += data.TotalPurchase;
                    totalDeliveryCostGlobal += data.DeliveryCost;
                    totalCostGlobal += data.TotalCost;
                }

                reportData.Add(new ReportEarningsData
                {
                    BusinessName = "Totales",
                    Month = filters.Year.ToString(),
                    TotalPurchase = totalPurchaseGlobal,
                    DeliveryCost = totalDeliveryCostGlobal,
                    TotalCost = totalCostGlobal
                });
            }

            return reportData;
        }
    }
}
