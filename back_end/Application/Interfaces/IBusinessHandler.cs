using back_end.Domain;

namespace back_end.Application.interfaces
{
    public interface IBusinessHandler
    {
        List<BusinessModel> getAllBusiness();
        List<BusinessModel> getBusinessByEmployeeID(string employeeID);
        List<BusinessAddressModel> getBusinessAddressByBusinessID(string businessID);
        bool insertNewBusiness(CompleteBusinessModel newBusiness);
        BusinessModel getBusinessByID(int businessID);
        List<MonthlyRevenueModel> GetMonthlyRevenueByBusinessID(int businessID);
    }
}