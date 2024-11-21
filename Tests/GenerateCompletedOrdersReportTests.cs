using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Domain;
using Moq;
using System.Data;

namespace Tests
{
    internal class GenerateCompletedOrdersReportTest
    {
        private Mock<IOrderHandler> _mockOrderHandler;
        private GenerateCompletedOrdersReport _generateCompletedOrdersReportCommand;

        [SetUp]
        public void Setup()
        {
            _mockOrderHandler = new Mock<IOrderHandler>();
            _generateCompletedOrdersReportCommand = new GenerateCompletedOrdersReport(_mockOrderHandler.Object);
        }

        [Test]
        public void Execute_ReportGeneratesCorrectly_WithValidInput()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters { ClientID = 1 };
            var reportData = new List<ReportCompletedOrderData>();
            var orderProducts = new List<ReportOrderProductData>
            {
                new ReportOrderProductData { OrderID = 1, Amount = 5, BusinessName = "Business A" },
                new ReportOrderProductData { OrderID = 1, Amount = 3, BusinessName = "Business B" },
                new ReportOrderProductData { OrderID = 2, Amount = 10, BusinessName = "Business C" }
            };

            // Mock GetReportOrderData
            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add("OrderID");
            reportDataTable.Columns.Add("ClientID");
            reportDataTable.Rows.Add(1, 1);
            reportDataTable.Rows.Add(2, 1);

            _mockOrderHandler
                .Setup(handler => handler.GetReportOrderData(It.IsAny<string>(), It.IsAny<ReportBaseFilters>(), out reportDataTable))
                .Returns(true);

            // Mock GetOrderProductsData
            var orderProductsTable = new DataTable();
            orderProductsTable.Columns.Add("OrderID");
            orderProductsTable.Columns.Add("Amount");
            orderProductsTable.Columns.Add("BusinessName");
            orderProductsTable.Rows.Add(1, 5, "Business A");
            orderProductsTable.Rows.Add(1, 3, "Business B");
            orderProductsTable.Rows.Add(2, 10, "Business C");

            _mockOrderHandler
                .Setup(handler => handler.GetOrderProductsData(It.IsAny<string>(), out orderProductsTable))
                .Returns(true);

            // Act
            var result = _generateCompletedOrdersReportCommand.Execute(baseFilters, out reportData);

            // Assert
            Assert.IsTrue(result);
            Assert.That(reportData.Count, Is.EqualTo(2));
            Assert.That(reportData[0].BusinessName, Is.EqualTo("Business A, Business B"));
            Assert.That(reportData[0].Amount, Is.EqualTo(8));
            Assert.That(reportData[1].BusinessName, Is.EqualTo("Business C"));
            Assert.That(reportData[1].Amount, Is.EqualTo(10));

            _mockOrderHandler.Verify(
                handler => handler.GetReportOrderData(It.IsAny<string>(), It.IsAny<ReportBaseFilters>(), out reportDataTable),
                Times.Once
            );
            _mockOrderHandler.Verify(
                handler => handler.GetOrderProductsData(It.IsAny<string>(), out orderProductsTable),
                Times.Once
            );
        }

        [Test]
        public void Execute_ReturnsFalse_WhenBaseFiltersAreInvalid()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters { ClientID = -1 };
            List<ReportCompletedOrderData> reportData;

            // Act
            var result = _generateCompletedOrdersReportCommand.Execute(baseFilters, out reportData);

            // Assert
            Assert.IsFalse(result);
            Assert.IsEmpty(reportData);
        }

        [Test]
        public void Execute_ReturnsFalse_WhenGetReportOrderDataFails()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters { ClientID = 1 };
            List<ReportCompletedOrderData> reportData;

            _mockOrderHandler
                .Setup(handler => handler.GetReportOrderData(It.IsAny<string>(), It.IsAny<ReportBaseFilters>(), out It.Ref<DataTable>.IsAny))
                .Returns(false);

            // Act
            var result = _generateCompletedOrdersReportCommand.Execute(baseFilters, out reportData);

            // Assert
            Assert.IsFalse(result);
            _mockOrderHandler.Verify(
                handler => handler.GetReportOrderData(It.IsAny<string>(), It.IsAny<ReportBaseFilters>(), out It.Ref<DataTable>.IsAny),
                Times.Once
            );
        }

        [Test]
        public void Execute_ReturnsFalse_WhenGetOrderProductsDataFails()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters { ClientID = 1 };
            List<ReportCompletedOrderData> reportData;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add("OrderID");
            reportDataTable.Columns.Add("ClientID");
            reportDataTable.Rows.Add(1, 1);
            reportDataTable.Rows.Add(2, 1);

            _mockOrderHandler
                .Setup(handler => handler.GetReportOrderData(It.IsAny<string>(), It.IsAny<ReportBaseFilters>(), out reportDataTable))
                .Returns(true);

            _mockOrderHandler
                .Setup(handler => handler.GetOrderProductsData(It.IsAny<string>(), out It.Ref<DataTable>.IsAny))
                .Returns(false);

            // Act
            var result = _generateCompletedOrdersReportCommand.Execute(baseFilters, out reportData);

            // Assert
            Assert.IsFalse(result);
            _mockOrderHandler.Verify(
                handler => handler.GetOrderProductsData(It.IsAny<string>(), out It.Ref<DataTable>.IsAny),
                Times.Never
            );
        }
    }
}
