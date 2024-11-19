using back_end.Domain;

namespace back_end.Application.Interfaces
{
    public interface IAdminHandler
    {
        List<MonthlyDataModel> GetMonthlyRevenue();
        List<MonthlyDataModel> GetMonthlyShippingExpense();
    }
}
