using back_end.Application.Interfaces;

namespace back_end.Application.Commands
{
    public class ProductDelete
    {
        private readonly IProductHandler productHandler;

        public ProductDelete(IProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }

        public bool DeleteProducts(List<int> productIds)
        {
            bool hardDeletedSuccessfull = false;
            bool softDeletedSuccessfull = false;
            List<int> productsInOrders = productHandler.GetInOrderProductsIds(productIds).Distinct().ToList();
            List<int> productsInShoppingCarts = productHandler.GetInShoppingCartProductsIds(productIds).Distinct().ToList();
            List<int> productsNotInOrdersNorShoppingCarts = productIds.Except(productsInOrders).Except(productsInShoppingCarts).ToList();
            productHandler.OpenSqlConnection();
            productHandler.BeginReadUncommittedTransaction();
            try
            {
                if (productsInOrders.Count > 0)
                    softDeletedSuccessfull = productHandler.SoftDeleteProducts(productsInOrders);
                else
                    softDeletedSuccessfull = true;

                if (productsNotInOrdersNorShoppingCarts.Count > 0)
                    hardDeletedSuccessfull = productHandler.HardDeleteProducts(productsNotInOrdersNorShoppingCarts);
                else
                    hardDeletedSuccessfull = true;
            }
            catch (Exception ex)
            {
                productHandler.RollbackTransaction();
                productHandler.CloseSqlConnection();
                Console.WriteLine($"Error deleting products: {ex.Message}. From method {ex.TargetSite}");
                throw;
            }
            productHandler.CommitTransaction();
            productHandler.CloseSqlConnection();
            return hardDeletedSuccessfull && softDeletedSuccessfull;
        }
    }
}
