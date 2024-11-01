
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using Moq;

namespace Tests
{
    internal class ProductSearchTests
    {
        [Test]
        public void countProductsBySearchWorksWithNullSearchText()
        {
            var productHandler = new Mock<IProductHandler>();
            const int productHandlermockReturnValue = 3;
            productHandler.Setup(handler =>
                handler.countProductsBySearch(It.IsAny<string>())).Returns(productHandlermockReturnValue);
            var productQuery = new ProductQuery(productHandler.Object);

            try
            {
                int productCount = productQuery.countProductsBySearch(null);
                Assert.AreEqual(productCount, productHandlermockReturnValue);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
    }
}
