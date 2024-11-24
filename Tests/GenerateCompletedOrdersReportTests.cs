using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Tests
{
    internal class GenerateCompletedOrdersReportTest
    {
        private Mock<IReportHandler> _mockReportHandler;
        private GenerateCompletedOrdersReport _command;

        [SetUp]
        public void Setup()
        {
            _mockReportHandler = new Mock<IReportHandler>();
            _command = new GenerateCompletedOrdersReport(_mockReportHandler.Object);
        }

        [Test]
        public void Execute_ShouldThrowArgumentNullException_WhenBaseFiltersAreNull()
        {
            // Arrange
            ReportBaseFilters? baseFilters = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _command.Execute(baseFilters));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenClientIDIsNegative()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters
            {
                ClientID = -1,
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(baseFilters));
        }

        [Test]
        public void Execute_ShouldThrowArgumentException_WhenStartDateIsAfterEndDate()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters
            {
                ClientID = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _command.Execute(baseFilters));
        }

        [Test]
        public void Execute_ShouldReturnEmptyList_WhenNoReportDataIsFetched()
        {
            // Arrange
            var baseFilters = new ReportBaseFilters
            {
                ClientID = 1,
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now
            };

            string query = string.Empty;

            var emptyTable = new DataTable();

            _mockReportHandler
                .Setup(r => r.FetchReportOrderData(query, baseFilters))
                .Returns(emptyTable);

            // Act
            var result = _command.Execute(baseFilters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
