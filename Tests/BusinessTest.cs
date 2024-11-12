using back_end.Application.Commands;
using back_end.Application.interfaces;
using back_end.Domain;
using Moq;

namespace Tests
{
    internal class BusinessTest
    {
        private Mock<IBusinessHandler> _mockBusinessHandler;
        private InsertNewBusiness _insertNewBusinessCommand;

        [SetUp]
        public void Setup()
        {
            _mockBusinessHandler = new Mock<IBusinessHandler>();
            _insertNewBusinessCommand = new InsertNewBusiness(_mockBusinessHandler.Object);
        }

        [Test]
        public void InsertNewBusiness_WorksWithStandardInput()
        {
            // Arrange
            var newBusiness = new CompleteBusinessModel
            {
                UserID = 1,
                BusinessID = 100,
                Name = "Test Business",
                IDNumber = "12345678",
                Email = "test@business.com",
                Telephone = "123456789",
                Permissions = "admin",
                Province = "ProvinceName",
                Canton = "CantonName",
                District = "DistrictName",
                PostalCode = "00000",
                OtherSigns = "Nearby landmark"
            };

            _mockBusinessHandler
                .Setup(handler => handler.insertNewBusiness(newBusiness))
                .Returns(true);

            // Act
            var result = _insertNewBusinessCommand.Execute(newBusiness);

            // Assert
            Assert.IsTrue(result);
            _mockBusinessHandler
                .Verify(
                    handler => handler.insertNewBusiness(newBusiness),
                    Times.Once
                );
        }

        [Test]
        public void InsertNewBusiness_InvalidBusinessID()
        {
            // Arrange
            var newBusiness = new CompleteBusinessModel
            {
                UserID = 1,
                BusinessID = null,
                Name = "Invalid Business",
                IDNumber = "00000000",
                Email = "invalid@business.com",
                Telephone = "000000000",
                Permissions = "user",
                Province = "InvalidProvince",
                Canton = "InvalidCanton",
                District = "InvalidDistrict",
                PostalCode = "99999",
                OtherSigns = "No landmark"
            };

            _mockBusinessHandler
                .Setup(handler => handler.insertNewBusiness(newBusiness))
                .Returns(false);

            // Act
            var result = _insertNewBusinessCommand.Execute(newBusiness);

            // Assert
            Assert.IsFalse(result);
            _mockBusinessHandler
                .Verify(
                    handler => handler.insertNewBusiness(newBusiness),
                    Times.Once
                );
        }
    }
}
