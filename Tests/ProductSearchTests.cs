
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
        public void CountProductsBySearchWorksWithNullSearchText()
        {
            const int handlerCountProductsBySearchMockReturn = 3;
            productHandlerMock.Setup(handler =>
                handler.CountProductsBySearch(It.IsAny<string>())).Returns(handlerCountProductsBySearchMockReturn);

            int productCount = productQuery.CountProductsBySearch(null);
            Assert.AreEqual(productCount, handlerCountProductsBySearchMockReturn);
        }

        [Test]
        public void SearchProductsDoesntCallHandlerWithNegativeStartIndex()
        {
            productHandlerMock.Setup(handler =>
                handler.SearchProducts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<ProductsSearchModel>());
            int startIndex = -2;
            int maxResults = 10;
            string searchText = "Example search";

            Assert.Throws<ArgumentException>(() => productQuery.SearchProducts(startIndex, maxResults, searchText));
        }
    }
}
