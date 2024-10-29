using back_end.Application.interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public class InsertNewBusiness
    {
        private readonly IBusinessHandler _businessHandler;

        public InsertNewBusiness(IBusinessHandler businessHandler)
        {
            _businessHandler = businessHandler;
        }

        public bool Execute(CompleteBusinessModel newBusiness)
        {
            return _businessHandler.insertNewBusiness(newBusiness);
        }
    }
}
