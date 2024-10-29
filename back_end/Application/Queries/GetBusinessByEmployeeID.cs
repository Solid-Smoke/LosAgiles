using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetBusinessByEmployeeID
    {
        private readonly IBusinessHandler _businessHandler;

        public GetBusinessByEmployeeID(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public List<BusinessModel> Execute(string employeeID)
        {
            return _businessHandler.getBusinessByEmployeeID(employeeID);
        }
    }
}
