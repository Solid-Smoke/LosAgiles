using back_end.Application.Interfaces;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Globalization;

namespace back_end.Application.Reports
{
    public class CompletedOrderReport : OrderReportTemplate<ReportCompletedOrderData>
    {
        public CompletedOrderReport(IReportHandler reportHandler)
            : base(reportHandler)
        {
        }

        public override byte[] GeneratePdf(List<ReportCompletedOrderData> reportData)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18);
                var tableFont = FontFactory.GetFont(FontFactory.TIMES, 12);

                document.Add(new Paragraph("Pedidos Completados por Tiempo", titleFont));
                document.Add(new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyy HH:mm:ss}", tableFont));
                document.Add(new Paragraph("\n"));

                // Table
                var table = new PdfPTable(9) { WidthPercentage = 100 }; // 9 columnas
                table.SetWidths(new float[] { 10, 15, 10, 15, 15, 15, 10, 10, 10 });

                // Header
                AddCellToHeader(table, "Número de orden");
                AddCellToHeader(table, "Emprendimientos asociados");
                AddCellToHeader(table, "Cantidad de items en la compra");
                AddCellToHeader(table, "Fecha de creación");
                AddCellToHeader(table, "Fecha de envío");
                AddCellToHeader(table, "Fecha de recibido");
                AddCellToHeader(table, "Costo total de los items");
                AddCellToHeader(table, "Costo de envío");
                AddCellToHeader(table, "Costo total de la compra");

                // Data
                foreach (var order in reportData)
                {
                    AddCellToBody(table, order.OrderID.ToString(), tableFont);
                    AddCellToBody(table, order.BusinessName ?? "N/A", tableFont);
                    AddCellToBody(table, order.Amount.ToString(), tableFont);
                    AddCellToBody(table, order.CreatedDate?.ToString("dd/MM/yyy") ?? "N/A", tableFont);
                    AddCellToBody(table, order.DeliveryDate?.ToString("dd/MM/yyy") ?? "N/A", tableFont);
                    AddCellToBody(table, order.ReceivedDate?.ToString("dd/MM/yyy") ?? "N/A", tableFont);
                    AddCellToBody(table, order.SubtotalCost.ToString("C", new CultureInfo("es-CR")), tableFont);
                    AddCellToBody(table, order.DeliveryCost.ToString("C", new CultureInfo("es-CR")), tableFont);
                    AddCellToBody(table, order.TotalCost.ToString("C", new CultureInfo("es-CR")), tableFont);

                }

                document.Add(table);
                document.Close();

                return memoryStream.ToArray();
            }
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
                                AND [o].StatusChangeDate BETWEEN @StartDate AND @EndDate
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
