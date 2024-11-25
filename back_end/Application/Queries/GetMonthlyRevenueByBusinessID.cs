using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetMonthlyRevenueByBusinessID
    {
        private readonly IBusinessHandler _businessHandler;

        public GetMonthlyRevenueByBusinessID(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public List<MonthlyRevenueModel> Execute(int businessID)
        {
            return _businessHandler.GetMonthlyRevenueByBusinessID(businessID);
        }
    }
}
