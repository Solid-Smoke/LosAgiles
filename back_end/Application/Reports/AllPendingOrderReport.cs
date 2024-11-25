using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Globalization;

namespace back_end.Application.Reports {
    public class AllPendingOrderReport : OrderReportTemplate<AdminReportOrderData> {
        public AllPendingOrderReport(IReportHandler reportHandler)
            : base(reportHandler) {
        }

        public override byte[] GeneratePdf(List<AdminReportOrderData> reportData, ReportBaseFilters baseFilters) {
            using (var pdfManager = new PDFManager()) {
                pdfManager.AddTitle("Pedidos Pendientes por Tiempo");
                pdfManager.AddParagraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                pdfManager.AddParagraph($"Fecha inicial del reporte: " + baseFilters.StartDate.ToShortDateString());
                pdfManager.AddParagraph($"Fecha final del reporte: " + baseFilters.EndDate.ToShortDateString());

                var columnWidths = new float[] { 10, 15, 15, 15, 15, 15, 15, 15, 15 };
                pdfManager.CreateTable(9, columnWidths);

                pdfManager.AddTableHeader("Número de orden");
                pdfManager.AddTableHeader("ID de usuario");
                pdfManager.AddTableHeader("Emprendimientos asociados");
                pdfManager.AddTableHeader("Cantidad de items en la compra");
                pdfManager.AddTableHeader("Fecha de creación");
                pdfManager.AddTableHeader("Fecha de envío");
                pdfManager.AddTableHeader("Costo total de los items");
                pdfManager.AddTableHeader("Costo de envío");
                pdfManager.AddTableHeader("Costo total de la compra");

                foreach (var order in reportData) {
                    pdfManager.AddTableBodyCell(order.OrderID.ToString());
                    pdfManager.AddTableBodyCell(order.UserID.ToString());
                    pdfManager.AddTableBodyCell(order.BusinessName ?? "N/A");
                    pdfManager.AddTableBodyCell(order.Amount.ToString());
                    pdfManager.AddTableBodyCell(order.CreatedDate?.ToString("dd/MM/yyyy") ?? "N/A");
                    pdfManager.AddTableBodyCell(order.DeliveryDate?.ToString("dd/MM/yyyy") ?? "N/A");
                    pdfManager.AddTableBodyCell("CRC " + order.SubtotalCost.ToString("#,##0.00", new CultureInfo("es-CR")));
                    pdfManager.AddTableBodyCell("CRC " + order.DeliveryCost.ToString("#,##0.00", new CultureInfo("es-CR")));
                    pdfManager.AddTableBodyCell("CRC " + order.TotalCost.ToString("#,##0.00", new CultureInfo("es-CR")));
                }

                pdfManager.AddTableToDocument();
                return pdfManager.GeneratePdf();
            }
        }

        protected override DataTable ExecuteReportQuery(ReportBaseFilters baseFilters) {
            string query = @"SELECT [o].OrderID, [o].ClientID, [o].CreatedDate, [o].DeliveryDate,
                            [o].SubtotalCost, [o].DeliveryCost, [o].TotalCost
                            FROM [Orders] [o]
                            WHERE [o].StatusChangeDate BETWEEN @StartDate AND @EndDate
                            AND [o].Status = 'Pendiente'";
            return _reportHandler.FetchReportOrderData(query, baseFilters);
        }

        protected override AdminReportOrderData MapOrderData(DataRow row) {
            return new AdminReportOrderData {
                OrderID = Convert.ToInt32(row["OrderID"]),
                UserID = Convert.ToInt32(row["ClientID"]),
                CreatedDate = row["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(row["CreatedDate"]) : null,
                DeliveryDate = row["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(row["DeliveryDate"]) : null,
                SubtotalCost = row["SubtotalCost"] != DBNull.Value ? Convert.ToDecimal(row["SubtotalCost"]) : 0,
                DeliveryCost = row["DeliveryCost"] != DBNull.Value ? Convert.ToDecimal(row["DeliveryCost"]) : 0,
                TotalCost = row["TotalCost"] != DBNull.Value ? Convert.ToDecimal(row["TotalCost"]) : 0
            };
        }
    }
}
