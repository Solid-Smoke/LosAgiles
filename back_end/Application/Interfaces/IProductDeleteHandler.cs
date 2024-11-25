namespace back_end.Application.Interfaces
{
    public interface IProductDeleteHandler
    {
        void BeginSerializableTransaction();
        void CloseSqlConnection();
        void CommitTransaction();
        List<int> GetActiveOrderProductsIds(List<int> productIds);
        List<int> GetInactiveOrderProductsIds(List<int> productIds);
        List<int> GetInShoppingCartProductsIds(List<int> productIds);
        bool HardDelete(List<int> productIds);
        void OpenSqlConnection();
        void RollbackTransaction();
        bool SoftDelete(List<int> productIds);
        bool DeleteFromShoppingCarts(List<int> productIds);
    }
}