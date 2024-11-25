using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;
using Moq;
using System.Data;


namespace ReportTests
{
    public class GenerateCompletedOrderReportPDFTests
    {
        private Mock<IReportHandler> _mockReportHandler;
        private GenerateCompletedOrderReportPDF _command;
        private Mock<CompletedOrderReport> _mockCompletedOrderReport;

        [SetUp]
        public void SetUp()
        {
            _mockReportHandler = new Mock<IReportHandler>();
            _mockCompletedOrderReport = new Mock<CompletedOrderReport>(_mockReportHandler.Object);
            _command = new GenerateCompletedOrderReportPDF(_mockCompletedOrderReport.Object);
        }

        [Test]
        public void Execute_ShouldThrowArgumentNullException_WhenFiltersAreNull()
        {
            // Arrange
            ReportBaseFilters? filters = null;
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _command.Execute(filters));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenClientIdIsNegative()
        {
            // Arrange
            var filters = new ReportBaseFilters
            {
                ClientID = -1,
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(filters));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenStartDateIsAfterEndDate()
        {
            // Arrange
            var filters = new ReportBaseFilters
            {
                ClientID = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(filters));
        }
    }
}
