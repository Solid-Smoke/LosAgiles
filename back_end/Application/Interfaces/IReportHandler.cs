using back_end.Domain;
using System.Data;

namespace back_end.Application.Interfaces
{
    public interface IReportHandler
    {
        public DataTable FetchReportOrderData(string query, ReportBaseFilters baseFilters);
        public DataTable FetchOrderProductsData(string orderIDs);
    }
}
