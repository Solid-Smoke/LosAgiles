using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetBusinessAddressByBusinessID
    {
        private readonly IBusinessHandler _businessHandler;

        public GetBusinessAddressByBusinessID(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public List<BusinessAddressModel> Execute(string businessID)
        {
            return _businessHandler.getBusinessAddressByBusinessID(businessID);
        }
    }
}