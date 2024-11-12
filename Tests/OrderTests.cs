using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using back_end.APIS;
using Moq;
using NUnit.Framework;

namespace back_end.Tests {
    public class OrderTests {
        private Mock<IOrderHandler> _mockOrderHandler;
        private ApproveOrder _approveOrder;
        private RejectOrder _rejectOrder;
        private GetPendingOrders _getPendingOrders;

        [SetUp]
        public void Setup() {
            _mockOrderHandler = new Mock<IOrderHandler>();
            _approveOrder = new ApproveOrder(_mockOrderHandler.Object);
            _rejectOrder = new RejectOrder(_mockOrderHandler.Object);
            _getPendingOrders = new GetPendingOrders(_mockOrderHandler.Object);
        }

        [Test]
        public void ApproveOrder_ValidOrderId_ReturnsTrue() {
            // Arrange
            var orderId = "1";
            _mockOrderHandler.Setup(handler => handler.ApproveOrder(orderId)).Returns(true);

            // Act
            var result = _approveOrder.Execute(orderId);

            // Assert
            Assert.IsTrue(result);
            _mockOrderHandler.Verify(handler => handler.ApproveOrder(orderId), Times.Once);
        }

        [Test]
        public void RejectOrder_ValidOrderId_ReturnsTrue() {
            // Arrange
            var orderId = "1";
            _mockOrderHandler.Setup(handler => handler.RejectOrder(orderId)).Returns(true);

            // Act
            var result = _rejectOrder.Execute(orderId);

            // Assert
            Assert.IsTrue(result);
            _mockOrderHandler.Verify(handler => handler.RejectOrder(orderId), Times.Once);
        }

        [Test]
        public void GetPendingOrders_ReturnsListOfPendingOrders() {
            // Arrange
            var expectedOrders = new List<OrderModel>
            {
                new OrderModel { OrderID = 1, Status = "Pendiente", Buyer = "Pedro", TotalAmount = 100, Address = "Aquí" },
                new OrderModel { OrderID = 2, Status = "Pendiente", Buyer = "Juan", TotalAmount = 200, Address = "Allá" }
            };
            _mockOrderHandler.Setup(handler => handler.GetPendingOrders()).Returns(expectedOrders);

            // Act
            var result = _getPendingOrders.Execute();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<OrderModel>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedOrders, result);
            _mockOrderHandler.Verify(handler => handler.GetPendingOrders(), Times.Once);
        }
    }
}
