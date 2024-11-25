using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetOrdersInProgressByBusinessID
    {
        private readonly IBusinessHandler _businessHandler;

        public GetOrdersInProgressByBusinessID(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public List<OrderModel> Execute(int businessID)
        {
            return _businessHandler.GetOrdersInProgressByBusinessID(businessID);
        }
    }
}
