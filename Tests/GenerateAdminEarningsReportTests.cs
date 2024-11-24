using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using Moq;
using System.Data;

namespace ReportTests
{
    public class GenerateAdminEarningsReportTests
    {
        private Mock<IReportHandler> _mockReportHandler;
        private GenerateAdminEarningsReport _command;

        [SetUp]
        public void SetUp()
        {
            _mockReportHandler = new Mock<IReportHandler>();
            _command = new GenerateAdminEarningsReport(_mockReportHandler.Object);
        }

        [Test]
        public void Execute_ShouldThrowArgumentNullException_WhenFiltersAreNull()
        {
            // Arrange
            ReportEarningsFilters? filters = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _command.Execute(filters));
            Assert.That(ex.Message, Is.EqualTo("Filters cannot be null. (Parameter 'filters')"));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenYearIsNegative()
        {
            // Arrange
            var filters = new ReportEarningsFilters
            {
                Year = -1
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(filters));
        }

        [Test]
        public void Execute_ShouldReturnEmptyList_WhenNoDataIsFetched()
        {
            // Arrange
            var filters = new ReportEarningsFilters
            {
                Year = 2023
            };
            _mockReportHandler
                .Setup(r => r.FetchEarningsReport(filters.Year, filters.BusinessID))
                .Returns(new DataTable());

            // Act
            var result = _command.Execute(filters);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
