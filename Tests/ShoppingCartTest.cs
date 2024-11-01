using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Domain;
using Moq;

namespace Tests
{
    internal class ShoppingCartTest
    {
        private Mock<IShoppingCartHandler> _mockShoppingCartHandler;
        private AddItemToShoppingCart _addItemToShoppingCartCommand;
        private DeleteInvalidProductsFromUserCart _deleteInvalidProductsCommand;

        [SetUp]
        public void Setup()
        {
            _mockShoppingCartHandler = new Mock<IShoppingCartHandler>(); // Setup, returns
            _addItemToShoppingCartCommand = new AddItemToShoppingCart(_mockShoppingCartHandler.Object);
            _deleteInvalidProductsCommand = new DeleteInvalidProductsFromUserCart(_mockShoppingCartHandler.Object);
        }

        [Test]
        public void AddCartItem_WorksWithStandartInput()
        {
            // Arrange
            var clientId = "1";
            var newItem = new ShoppingCartItemModel
            {
                ProductID = 1,
                Amount = 2,
            };

            _mockShoppingCartHandler
                .Setup(
                    handler => handler.AddCartItem(clientId, newItem)
                )
                .Returns(true);

            // Act
            var result = _addItemToShoppingCartCommand.Execute(clientId, newItem);

            // Assert
            Assert.IsTrue(result);
            _mockShoppingCartHandler
                .Verify(
                    handler => handler.AddCartItem(clientId, newItem),
                    Times.Once
                );
        }

        [Test]
        public void AddCartItem_BadClientIDInput()
        {
            // Arrange
            var clientId = "A";
            var newItem = new ShoppingCartItemModel
            {
                ProductID = 1,
                Amount = 2,
            };

            _mockShoppingCartHandler
                .Setup(
                    handler => handler.AddCartItem(clientId, newItem)
                )
                .Returns(false);

            // Act
            var result = _addItemToShoppingCartCommand.Execute(clientId, newItem);

            // Assert
            Assert.IsFalse(result);
            _mockShoppingCartHandler
                .Verify(
                    handler => handler.AddCartItem(clientId, newItem),
                    Times.Once
                );
        }

        [Test]
        public void DeleteInvalidProducts_WorksWithMoreThanOneItem()
        {
            // Arrange
            var clientId = "1";
            var itemsToDelete = new List<ShoppingCartItemDataModel>
            {
                new ShoppingCartItemDataModel { ProductID = 1 },
                new ShoppingCartItemDataModel { ProductID = 2 },
                new ShoppingCartItemDataModel { ProductID = 3 }
            };

            // Act
            _deleteInvalidProductsCommand.Execute(clientId, itemsToDelete);

            // Assert
            foreach (var item in itemsToDelete)
            {
                _mockShoppingCartHandler
                    .Verify(
                        handler => handler.DeleteItemFromCart(clientId, item.ProductID),
                        Times.Once
                    );
            }
        }
    }
}
