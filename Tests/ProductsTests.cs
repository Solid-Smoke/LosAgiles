using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using back_end.APIS;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Tests
{
    public class ProductTests
    {
        private Mock<IProductHandler> _mockProductHandler;
        private Mock<IProductDelete> _mockProductDeleteCommand;
        private ProductCommand _productCommand;
        private ProductQuery _productQuery;
        private ProductsController _productsController;

        [SetUp]
        public void Setup()
        {
            _mockProductHandler = new Mock<IProductHandler>();
            _mockProductDeleteCommand = new Mock<IProductDelete>();
            _productCommand = new ProductCommand(_mockProductHandler.Object);
            _productQuery = new ProductQuery(_mockProductHandler.Object);
            _productsController = new ProductsController(_productQuery, _mockProductHandler.Object, _mockProductDeleteCommand.Object);
        }

        [Test]
        public async Task CreateProduct_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var product = new ProductModel { Name = "Test Product", Price = 10, Stock = 100 };
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.Setup(request => request.Form.Files.Count).Returns(0);
            _mockProductHandler.Setup(handler => handler.CreateProduct(It.IsAny<ProductModel>())).Returns(true);

            // Act
            var result = await _productCommand.CreateProduct(product, mockHttpRequest.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetProductById_ExistingProductId_ReturnsProduct()
        {
            // Arrange
            int productId = 1;
            var product = new ProductModel { ProductID = productId, Name = "Existing Product" };
            _mockProductHandler.Setup(handler => handler.GetProductById(productId)).Returns(product);

            // Act
            var result = _productsController.GetProductById(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result.Result);
            Assert.AreEqual(product, ((JsonResult)result.Result).Value as ProductModel);
        }

        [Test]
        public void GetProductById_NonExistentProductId_ReturnsObjectResultWithServerError()
        {
            // Arrange
            int productId = 999;
            _mockProductHandler.Setup(handler => handler.GetProductById(productId)).Returns((ProductModel)null);

            // Act
            var result = _productsController.GetProductById(productId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult?.StatusCode);
        }

        [Test]
        public void GetAllProducts_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<ProductModel>
            {
                new ProductModel { ProductID = 1, Name = "Product 1" },
                new ProductModel { ProductID = 2, Name = "Product 2" }
            };
            _mockProductHandler.Setup(handler => handler.GetAllProducts()).Returns(products);

            // Act
            var result = _productsController.GetAllProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result.Result);
            var resultList = ((JsonResult)result.Result).Value as List<ProductModel>;
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
        }
    }
}
