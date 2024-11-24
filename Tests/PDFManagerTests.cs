using iTextSharp.text;
using NUnit.Framework;
using System.IO;

namespace ReportTests
{
    [TestFixture]
    public class PDFManagerTests
    {
        private PDFManager CreateNewPdfManager()
        {
            return new PDFManager();
        }

        [Test]
        public void AddTableToDocument_WithoutCreatingTable_ShouldThrowException()
        {
            // Arrange
            var pdfManager = CreateNewPdfManager();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                pdfManager.AddTableToDocument();
            });
        }

        [Test]
        public void AddTableHeader_WithoutCreatingTable_ShouldThrowException()
        {
            // Arrange
            var pdfManager = CreateNewPdfManager();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                pdfManager.AddTableHeader("Header");
            });
        }

        [Test]
        public void AddTableBodyCell_WithoutCreatingTable_ShouldThrowException()
        {
            // Arrange
            var pdfManager = CreateNewPdfManager();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                pdfManager.AddTableBodyCell("Cell Text");
            });
        }
    }
}
