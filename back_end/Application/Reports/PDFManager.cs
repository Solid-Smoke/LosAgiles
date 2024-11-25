using iTextSharp.text;
using iTextSharp.text.pdf;

public class PDFManager : IDisposable
{
    private readonly Document _document;
    private readonly MemoryStream _memoryStream;
    private PdfPTable? _currentTable;

    public Font TitleFont { get; }
    public Font ParagraphFont { get; }

    public PDFManager()
    {
        _memoryStream = new MemoryStream();
        _document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
        PdfWriter.GetInstance(_document, _memoryStream);
        _document.Open();

        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
        TitleFont = new Font(baseFont, 18, Font.BOLD);
        ParagraphFont = new Font(baseFont, 12);
    }

    public void AddTitle(string title)
    {
        _document.Add(new Paragraph(title + "\n\n", TitleFont));
    }

    public void AddParagraph(string content)
    {
        _document.Add(new Paragraph(content + "\n\n", ParagraphFont));
    }

    public void CreateTable(int columnCount, float[]? columnWidths = null)
    {
        _currentTable = new PdfPTable(columnCount) { WidthPercentage = 100 };
        if (columnWidths != null)
        {
            _currentTable.SetWidths(columnWidths);
        }
    }

    public void AddTableHeader(string headerText)
    {
        if (_currentTable == null)
        {
            throw new InvalidOperationException("Table isnt created or added to this document.(AddTableHeader)");
        }

        var headerFont = new Font(TitleFont.Family, 12, Font.BOLD, BaseColor.WHITE);
        var cell = new PdfPCell(new Phrase(headerText, headerFont))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };
        _currentTable.AddCell(cell);
    }

    public void AddTableBodyCell(string cellText, bool? isBold = false)
    {
        if (_currentTable == null)
        {
            throw new InvalidOperationException("Table isn't created or added to this document. (AddTableBodyCell)");
        }

        var fontToUse = isBold.HasValue && isBold.Value
            ? FontFactory.GetFont(ParagraphFont.Familyname, ParagraphFont.Size, Font.BOLD)
            : ParagraphFont;

        var cell = new PdfPCell(new Phrase(cellText, fontToUse))
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };

        _currentTable.AddCell(cell);
    }

    public void AddTableToDocument()
    {
        if (_currentTable == null)
        {
            throw new InvalidOperationException("Table isnt created or added to this document.(AddTableToDocument)");
        }

        _document.Add(_currentTable);
        _currentTable = null;
    }

    public byte[] GeneratePdf()
    {
        _document.Close();
        return _memoryStream.ToArray();
    }

    public void Dispose()
    {
        _document.Dispose();
        _memoryStream.Dispose();
    }
}
