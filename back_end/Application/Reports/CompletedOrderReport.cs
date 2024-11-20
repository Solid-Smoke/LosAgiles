using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using System.Data;

namespace back_end.Application.Reports
{
    public class CompletedOrderReport : OrderReportTemplate<ReportCompletedOrderData>
    {
        public CompletedOrderReport(IReportHandler reportHandler)
            : base(reportHandler)
        {
        }

        protected override DataTable ExecuteReportQuery(ReportBaseFilters baseFilters)
        {
            string query = @"SELECT 
                                [o].OrderID, [o].CreatedDate, [o].DeliveryDate, [o].ReceivedDate,
                                [o].SubtotalCost, [o].DeliveryCost, [o].TotalCost
                             FROM 
                                [Orders] [o]
                            WHERE
                                [o].ClientID = @ClientID 
                                AND [o].ReceivedDate BETWEEN @StartDate AND @EndDate
                                AND [o].Status = 'Completada'";
            return _reportHandler.FetchReportOrderData(query, baseFilters);
        }

        protected override ReportCompletedOrderData MapOrderData(DataRow row)
        {
            return new ReportCompletedOrderData
            {
                OrderID = Convert.ToInt32(row["OrderID"]),
                CreatedDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : null,
                DeliveryDate = row["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(row["DeliveryDate"]) : null,
                ReceivedDate = row["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(row["ReceivedDate"]) : null,
                SubtotalCost = row["SubtotalCost"] != DBNull.Value ? Convert.ToDecimal(row["SubtotalCost"]) : 0,
                DeliveryCost = row["DeliveryCost"] != DBNull.Value ? Convert.ToDecimal(row["DeliveryCost"]) : 0,
                TotalCost = row["TotalCost"] != DBNull.Value ? Convert.ToDecimal(row["TotalCost"]) : 0
            };
        }
    }

}
