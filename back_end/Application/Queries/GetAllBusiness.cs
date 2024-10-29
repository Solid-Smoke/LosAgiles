using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Queries
{
    public class GetAllBusiness
    {
        private readonly IBusinessHandler _businessHandler;

        public GetAllBusiness(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public List<BusinessModel> Execute()
        {
            return _businessHandler.getAllBusiness();
        }
    }
}
