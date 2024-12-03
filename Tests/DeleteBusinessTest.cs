using back_end.Application.Commands;
using back_end.Application.Interfaces;
using Moq;

namespace Tests
{
    internal class DeleteBusinessTest
    {
        private Mock<IBusinessDeleteHandler> _mockBusinessDeleteHandler;
        private Mock<IProductDelete> _mockProductDeleteCommand;
        private BusinessDelete _businessDeleteCommand;

        [SetUp]
        public void Setup()
        {
            _mockBusinessDeleteHandler = new Mock<IBusinessDeleteHandler>();
            _mockProductDeleteCommand = new Mock<IProductDelete>();
            _businessDeleteCommand = new BusinessDelete(_mockBusinessDeleteHandler.Object, _mockProductDeleteCommand.Object);
        }

        [Test]
        public void DeleteBusiness_prevents_businessId_lower_than_1()
        {
            var invalidBusinessId = 0;
            Assert.Throws<ArgumentException>(() => _businessDeleteCommand.DeleteBusiness(invalidBusinessId));
        }
    }
}
