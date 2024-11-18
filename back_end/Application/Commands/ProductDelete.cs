using back_end.Application.Interfaces;

namespace back_end.Application.Commands
{
    public class ProductDelete
    {
        private readonly IProductDeleteHandler productDeleteHandler;

        public ProductDelete(IProductDeleteHandler productHandler)
        {
            this.productDeleteHandler = productHandler;
        }

        private void CheckIfAreProductsInActiveOrders(List<int> productIds)
        {
            List<int> activeOrdersProducts = productDeleteHandler.GetActiveOrderProductsIds(productIds).Distinct().ToList();
            Exception cannotDeleteActiveOrdersProductsException = new Exception(
                $"Cannot delete products that are in active orders. Conflicting product IDs: {activeOrdersProducts.ToString()}");
            if (activeOrdersProducts.Count > 0)
            {
                cannotDeleteActiveOrdersProductsException.Data.Add("ProductIds", activeOrdersProducts);
                throw cannotDeleteActiveOrdersProductsException;
            }

        }

        private void ExecuteDeleteStatements(List<int> inactiveOrdersProducts, List<int> productsInShoppingCarts, List<int> productsNotInOrders)
        {
            if (productsInShoppingCarts.Count > 0)
                productDeleteHandler.DeleteFromShoppingCarts(productsInShoppingCarts);
            if (inactiveOrdersProducts.Count > 0)
                productDeleteHandler.SoftDelete(inactiveOrdersProducts);
            if (productsNotInOrders.Count > 0)
                productDeleteHandler.HardDelete(productsNotInOrders);
            
        }

        public void DeleteProducts(List<int> productIds)
        {
            productDeleteHandler.OpenSqlConnection();
            productDeleteHandler.BeginSerializableTransaction();
            CheckIfAreProductsInActiveOrders(productIds);
            List<int> inactiveOrdersProducts = productDeleteHandler.GetInactiveOrderProductsIds(productIds).Distinct().ToList();
            List<int> productsInShoppingCarts = productDeleteHandler.GetInShoppingCartProductsIds(productIds).Distinct().ToList();
            List<int> productsNotInOrders = productIds.Except(inactiveOrdersProducts).ToList();
            try
            {
                ExecuteDeleteStatements(inactiveOrdersProducts, productsInShoppingCarts, productsNotInOrders);
            }
            catch (Exception ex)
            {
                productDeleteHandler.RollbackTransaction();
                productDeleteHandler.CloseSqlConnection();
                Console.WriteLine($"Error deleting products: {ex.Message}");
                throw;
            }
            productDeleteHandler.CommitTransaction();
            productDeleteHandler.CloseSqlConnection();
        }
    }
}
