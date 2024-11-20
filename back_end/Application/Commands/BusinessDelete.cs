using back_end.Application.Interfaces;

namespace back_end.Application.Commands
{
    public interface IBusinessDelete
    {
        void DeleteBusiness(int businessId);
    }

    public class BusinessDelete : IBusinessDelete
    {
        IBusinessDeleteHandler businessDeleteHandler;
        IProductDelete productDeleteCommand;
        public BusinessDelete(IBusinessDeleteHandler businessHandler, IProductDelete productDeleteCommand)
        {
            this.businessDeleteHandler = businessHandler;
            this.productDeleteCommand = productDeleteCommand;
        }

        private void DeleteReferencesInOtherTables(int businessId)
        {
            List<int> businessProducts = businessDeleteHandler.GetBusinessProductsIds(businessId);
            if (businessProducts.Count > 0)
                productDeleteCommand.DeleteProducts(businessProducts);
            businessDeleteHandler.DeleteEmployees(businessId);
            businessDeleteHandler.DeleteBusinessAddress(businessId);
        }

        public void DeleteBusiness(int businessId)
        {
            if (businessId < 1)
                throw new ArgumentException("businessId must be greater than 1");
            businessDeleteHandler.OpenSqlConnection();
            businessDeleteHandler.BeginTransaction();
            businessDeleteHandler.SetTransactionIsolationLevel("SERIALIZABLE");
            try
            {
                DeleteReferencesInOtherTables(businessId);
                if (businessDeleteHandler.CheckIfAProductWasSoftDeleted(businessId))
                    businessDeleteHandler.SoftDeleteBusiness(businessId);
                else
                    businessDeleteHandler.HardDeleteBusiness(businessId);
                businessDeleteHandler.CommitTransaction();
                businessDeleteHandler.CloseSqlConnection();
            }
            catch (Exception ex)
            {
                businessDeleteHandler.RollbackTransaction();
                businessDeleteHandler.CloseSqlConnection();
                throw;
            }
        }
    }
}
