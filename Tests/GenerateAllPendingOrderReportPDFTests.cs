using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;
using Moq;

namespace Tests {
    internal class GenerateAllPendingOrderReportPDFTests {
        private Mock<IReportHandler> _mockReportHandler;
        private GenerateAllPendingOrderReportPDF _command;
        private Mock<AllPendingOrderReport> _mockAllPendingOrderReport;

        [SetUp]
        public void SetUp() {
            _mockReportHandler = new Mock<IReportHandler>();
            _mockAllPendingOrderReport = new Mock<AllPendingOrderReport>(_mockReportHandler.Object);
            _command = new GenerateAllPendingOrderReportPDF(_mockAllPendingOrderReport.Object);
        }

        [Test]
        public void Execute_ShouldThrowArgumentNullException_WhenFiltersAreNull() {
            // Arrange
            ReportBaseFilters? filters = null;
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _command.Execute(filters));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenStartDateIsAfterEndDate() {
            // Arrange
            var filters = new ReportBaseFilters {
                ClientID = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(filters));
        }
    }
}
