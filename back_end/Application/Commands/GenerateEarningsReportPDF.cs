using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class GenerateEarningsReportPDF
    {
        private readonly EarningsReportTemplate<ReportEarningsFilters> _earningsReportTemplate;

        public GenerateEarningsReportPDF(
            EarningsReportTemplate<ReportEarningsFilters> earningsReportTemplate)
        {
            _earningsReportTemplate = earningsReportTemplate;
        }

        public byte[] Execute(ReportEarningsFilters filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters), "Filters cannot be null.");
            if (filters.Year < 0)
                throw new ArgumentException("Year must be a non-negative value.", nameof(filters));

            List<ReportEarningsData> reportData = _earningsReportTemplate.FetchEarningsReport(filters);

            if (reportData.Count > 0)
            {
                return _earningsReportTemplate.GeneratePdf(reportData, filters);
            }
            return new byte[0];
        }
    }
}
