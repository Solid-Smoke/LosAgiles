﻿using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateAllCompletedOrderReportPDF
    {
        private readonly AllCompletedOrderReport _allCompletedOrderReport;
        public GenerateAllCompletedOrderReportPDF(
            AllCompletedOrderReport reportHandler)
        {
            _allCompletedOrderReport = reportHandler;
        }

        public byte[] Execute(ReportBaseFilters baseFilters)
        {
            if (baseFilters == null)
                throw new ArgumentNullException(nameof(baseFilters), "Base filters cannot be null.");
            if (baseFilters.StartDate > baseFilters.EndDate)
                throw new ArgumentException("StartDate cannot be later than EndDate.", nameof(baseFilters));
            List<AdminReportOrderData> reportData = _allCompletedOrderReport.FetchReportOrders(baseFilters);

            if (reportData.Count > 0)
            {
                string orderIDs = _allCompletedOrderReport.GetOrderIDsFromReport(reportData.AsEnumerable());
                List<ReportOrderProductData> orderProducts = _allCompletedOrderReport.FetchOrderProducts(orderIDs);
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
                return _allCompletedOrderReport.GeneratePdf(reportData, baseFilters);
            }
            return new byte[0];
        }
    }
}
