using back_end.Application.Interfaces;
using back_end.Domain;
using System.Data;

public abstract class EarningsReportTemplate<T> where T : ReportEarningsFilters
{
    protected readonly IReportHandler _reportHandler;

    public EarningsReportTemplate(IReportHandler reportHandler)
    {
        _reportHandler = reportHandler;
    }

    public List<ReportEarningsData> FetchEarningsReport(T filters)
    {
        if (filters == null)
            throw new ArgumentNullException(nameof(filters), "Filters cannot be null.");
        if (filters.Year < 0)
            throw new ArgumentException("Year must be a non-negative value.", nameof(filters));

        try
        {
            DataTable queryResultTable = ExecuteEarningsQuery(filters);
            return ProcessQueryResult(queryResultTable);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while fetching earnings report data.", ex);
        }
    }

    public abstract byte[] GeneratePdf(List<ReportEarningsData> reportData, T filters);

    protected abstract DataTable ExecuteEarningsQuery(T filters);

    protected abstract ReportEarningsData MapEarningsData(DataRow row);

    private List<ReportEarningsData> ProcessQueryResult(DataTable queryResultTable)
    {
        var earningsReport = new List<ReportEarningsData>();

        foreach (DataRow row in queryResultTable.Rows)
        {
            var earningsData = MapEarningsData(row);
            earningsReport.Add(earningsData);
        }

        return earningsReport;
    }
}
