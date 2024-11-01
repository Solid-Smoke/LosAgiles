
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using Moq;

namespace Tests
{
    internal class ProductSearchTests
    {
        Mock<IProductHandler> productHandlerMock;
        ProductQuery productQuery;

        [SetUp]
        public void Setup()
        {
            productHandlerMock = new Mock<IProductHandler>();
            productQuery = new ProductQuery(productHandlerMock.Object);
        }

        [Test]
        public void countProductsBySearchWorksWithNullSearchText()
        {
            const int handlerCountProductsBySearchMockReturn = 3;
            productHandlerMock.Setup(handler =>
                handler.countProductsBySearch(It.IsAny<string>())).Returns(handlerCountProductsBySearchMockReturn);

            int productCount = productQuery.countProductsBySearch(null);
            Assert.AreEqual(productCount, handlerCountProductsBySearchMockReturn);
        }

        [Test]
        public void searchProductsDoesntWorkWithNegativeStartIndex()
        {
            productHandlerMock.Setup(handler =>
                handler.searchProducts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<ProductsSearchModel>());

            try
            {
                int startIndex = -2;
                int maxResults = 10;
                string searchText = "Example search";
                productQuery.searchProducts(startIndex, maxResults, searchText);
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}
