namespace back_end.Application.Interfaces
{
    public interface IBusinessDeleteHandler
    {
        void BeginTransaction();
        bool CheckIfAProductWasSoftDeleted(int businessId);
        void CloseSqlConnection();
        void CommitTransaction();
        void DeleteBusinessAddress(int businessId);
        void DeleteEmployees(int businessId);
        List<int> GetBusinessProductsIds(int businessId);
        void HardDeleteBusiness(int businessId);
        void OpenSqlConnection();
        void RollbackTransaction();
        void SetTransactionIsolationLevel(string isolationLevel);
        void SoftDeleteBusiness(int businessId);
    }
}