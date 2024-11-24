using back_end.Application.Commands;
using back_end.Application.Interfaces;
using back_end.Application.Reports;
using back_end.Domain;
using Moq;

namespace Tests {
    internal class GenerateAllCancelledOrderReportPDFTests {
        private Mock<IReportHandler> _mockReportHandler;
        private GenerateAllCancelledOrderReportPDF _command;
        private Mock<AllCancelledOrderReport> _mockAllCancelledOrderReport;

        [SetUp]
        public void SetUp() {
            _mockReportHandler = new Mock<IReportHandler>();
            _mockAllCancelledOrderReport = new Mock<AllCancelledOrderReport>(_mockReportHandler.Object);
            _command = new GenerateAllCancelledOrderReportPDF(_mockAllCancelledOrderReport.Object);
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
