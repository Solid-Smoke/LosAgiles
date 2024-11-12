

using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Domain;
using Moq;

namespace Tests
{
    internal class CreateOrderTests
    {
        Mock<IOrderHandler> orderHandlerMock;
        SubmitOrder orderSubmitter;
        [SetUp]
        public void SetUp()
        {
            orderHandlerMock = new Mock<IOrderHandler>();
            orderSubmitter = new SubmitOrder(orderHandlerMock.Object);
        }
        [Test]
        public void CreateOrderDoesntCallHandlerWithNullProductList()
        {
            var orderData = new CreateOrderModel();
            orderData.Products = null;

            Assert.Throws<ArgumentException>(() => orderSubmitter.createOrder(orderData));           
        }

    }
}
