using back_end.Application.Interfaces;

namespace back_end.Application.Commands
{
    public interface IProductDelete
    {
        void DeleteProducts(List<int> productIds);
        void Execute(List<int> productIds);
    }

    public class ProductDelete : IProductDelete
    {
        private readonly IProductDeleteHandler productDeleteHandler;

        public ProductDelete(IProductDeleteHandler productHandler)
        {
            this.productDeleteHandler = productHandler;
        }

        private void CheckIfAreProductsInActiveOrders(List<int> productIds)
        {
            List<int> activeOrdersProducts = productDeleteHandler.GetActiveOrderProductsIds(productIds).Distinct().ToList();
            if (activeOrdersProducts.Count > 0)
            {
                Exception cannotDeleteActiveOrdersProductsException = new Exception(
                    $"Cannot delete products that are in active orders. Conflicting product IDs: {activeOrdersProducts.ToString()}");
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
            if (productIds == null || productIds.Count == 0)
            {
                throw new ArgumentException("The product list for deleting cannot be empty");
            }

            foreach (var productId in productIds)
            {
                if (productId <= 0)
                {
                    throw new ArgumentException("Product IDs have to be greater than zero");
                }
            }

            CheckIfAreProductsInActiveOrders(productIds);
            List<int> inactiveOrdersProducts = productDeleteHandler.GetInactiveOrderProductsIds(productIds).Distinct().ToList();
            List<int> productsInShoppingCarts = productDeleteHandler.GetInShoppingCartProductsIds(productIds).Distinct().ToList();
            List<int> productsNotInOrders = productIds.Except(inactiveOrdersProducts).ToList();
            ExecuteDeleteStatements(inactiveOrdersProducts, productsInShoppingCarts, productsNotInOrders);
        }

        public void Execute(List<int> productIds)
        {
            if (productIds == null || productIds.Count == 0)
            {
                throw new ArgumentException("The product list for deleting cannot be empty");
            }

            foreach (var productId in productIds)
            {
                if (productId <= 0)
                {
                    throw new ArgumentException("Product IDs have to be greater than zero");
                }
            }
            productDeleteHandler.OpenSqlConnection();
            productDeleteHandler.BeginSerializableTransaction();
            try
            {
                DeleteProducts(productIds);
            }
            catch
            {
                productDeleteHandler.RollbackTransaction();
                productDeleteHandler.CloseSqlConnection();
                throw;
            }
            productDeleteHandler.CommitTransaction();
            productDeleteHandler.CloseSqlConnection();
        }
    }
}
