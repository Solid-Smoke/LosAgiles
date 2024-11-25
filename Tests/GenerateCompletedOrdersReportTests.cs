using back_end.Application.Interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using Moq;
using System.Data;

namespace Tests
{
    internal class GenerateCompletedOrdersReportTest
    {
        private Mock<IReportHandler> _mockOrderHandler;
        private GenerateCompletedOrdersReport _generateCompletedOrdersReportCommand;

        [SetUp]
        public void Setup()
        {
            _mockOrderHandler = new Mock<IReportHandler>();
            _generateCompletedOrdersReportCommand = new GenerateCompletedOrdersReport(_mockOrderHandler.Object);
        }


    }
}
