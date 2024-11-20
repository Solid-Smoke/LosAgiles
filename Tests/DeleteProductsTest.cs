using back_end.Application.Commands;
using back_end.Application.Interfaces;
using Moq;

namespace Tests
{
    internal class DeleteProductsTest
    {
        private Mock<IProductDeleteHandler> _mockProductDeleteHandler;
        private ProductDelete _productDeleteCommand;

        [SetUp]
        public void Setup()
        {
            _mockProductDeleteHandler = new Mock<IProductDeleteHandler>();
            _productDeleteCommand = new ProductDelete(_mockProductDeleteHandler.Object);
        }

        [Test]
        public void DeleteProducts_prevents_empty_product_list()
        {
            var emptyProductList = new List<int>();
            Assert.Throws<ArgumentException>(() => _productDeleteCommand.DeleteProducts(emptyProductList));
        }

        [Test]
        public void DeleteProducts_prevents_negative_or_zero_as_product_IDs_in_product_list()
        {
            var invalidProductList = new List<int> { -1, 0, 2 };
            Assert.Throws<ArgumentException>(() => _productDeleteCommand.DeleteProducts(invalidProductList));
        }

        [Test]
        public void Execute_prevents_empty_product_list()
        {
            var emptyProductList = new List<int>();
            Assert.Throws<ArgumentException>(() => _productDeleteCommand.Execute(emptyProductList));
        }

        [Test]
        public void Execute_prevents_negative_or_zero_as_product_IDs_in_product_list()
        {
            var invalidProductList = new List<int> { -1, 0, 2 };
            Assert.Throws<ArgumentException>(() => _productDeleteCommand.Execute(invalidProductList));
        }
    }
}
}
