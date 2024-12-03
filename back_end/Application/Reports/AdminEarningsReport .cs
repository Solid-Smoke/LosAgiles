using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;
using System.Globalization;

public class AdminEarningsReport : EarningsReportTemplate<ReportEarningsFilters>
{
    public AdminEarningsReport(IReportHandler reportHandler) : base(reportHandler) { }

    protected override DataTable ExecuteEarningsQuery(ReportEarningsFilters filters)
    {
        return _reportHandler.FetchEarningsReport(filters.Year, filters.BusinessID);
    }

    protected override ReportEarningsData MapEarningsData(DataRow row)
    {
        return new ReportEarningsData
        {
            BusinessName = row["BusinessName"]?.ToString(),
            Month = row["Month"]?.ToString(),
            TotalPurchase = Convert.ToDecimal(row["TotalPurchase"]),
            DeliveryCost = Convert.ToDecimal(row["DeliveryCost"]),
            TotalCost = Convert.ToDecimal(row["TotalCost"])
        };
    }

    public override byte[] GeneratePdf(List<ReportEarningsData> reportData, ReportEarningsFilters filters)
    {
        using (var pdfManager = new PDFManager())
        {
            pdfManager.AddTitle("Reporte de Ganancias por Año y Mes");
            pdfManager.AddParagraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            pdfManager.AddParagraph($"Año del reporte: {filters.Year}");

            if (filters.BusinessID.HasValue)
            {
                pdfManager.AddParagraph($"ID del negocio: {filters.BusinessID.Value}");
            }
            else
            {
                pdfManager.AddParagraph("Reporte incluye todos los negocios.");
            }

            var columnWidths = new float[] { 25, 15, 15, 15, 15 };
            pdfManager.CreateTable(5, columnWidths);

            pdfManager.AddTableHeader("Emprendimiento");
            pdfManager.AddTableHeader("Mes");
            pdfManager.AddTableHeader("Total de Compra");
            pdfManager.AddTableHeader("Total de Envío");
            pdfManager.AddTableHeader("Costo Total de la Compra");

            decimal totalPurchaseGlobal = 0;
            decimal totalDeliveryCostGlobal = 0;
            decimal totalCostGlobal = 0;

            foreach (var data in reportData)
            {
                pdfManager.AddTableBodyCell(data.BusinessName ?? "N/A");
                pdfManager.AddTableBodyCell(data.Month ?? "N/A");
                pdfManager.AddTableBodyCell("CRC " + data.TotalPurchase.ToString("#,##0.00", new CultureInfo("es-CR")));
                pdfManager.AddTableBodyCell("CRC " + data.DeliveryCost.ToString("#,##0.00", new CultureInfo("es-CR")));
                pdfManager.AddTableBodyCell("CRC " + data.TotalCost.ToString("#,##0.00", new CultureInfo("es-CR")));

                totalPurchaseGlobal += data.TotalPurchase;
                totalDeliveryCostGlobal += data.DeliveryCost;
                totalCostGlobal += data.TotalCost;
            }

            pdfManager.AddTableBodyCell("Totales", isBold: true);
            pdfManager.AddTableBodyCell(filters.Year.ToString(), isBold: true);
            pdfManager.AddTableBodyCell("CRC " + totalPurchaseGlobal.ToString("#,##0.00", new CultureInfo("es-CR")), isBold: true);
            pdfManager.AddTableBodyCell("CRC " + totalDeliveryCostGlobal.ToString("#,##0.00", new CultureInfo("es-CR")), isBold: true);
            pdfManager.AddTableBodyCell("CRC " + totalCostGlobal.ToString("#,##0.00", new CultureInfo("es-CR")), isBold: true);

            pdfManager.AddTableToDocument();

            return pdfManager.GeneratePdf();
        }
    }

}
