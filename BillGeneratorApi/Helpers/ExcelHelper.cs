using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using OfficeOpenXml;

namespace billGenerator.Helpers
{
    public static class ExcelHelper
    {
        // reads excel and extracts students
        public static async Task<List<Student>> ProcessExcelFile(IFormFile file)
        {
            var Students = new List<Student>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // First worksheet
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 (to skip header)
                    {
                        var Student = new Student
                        {
                            Name = worksheet.Cells[row, 1].Text,
                            Email = worksheet.Cells[row, 2].Text,
                            No = decimal.Parse(worksheet.Cells[row, 3].Text),
                            Parent = worksheet.Cells[row, 4].Text
                        };
                        Students.Add(Student);
                    }
                }
            }
            return Students;
        }

        public static byte[] GeneratePdf(List<Student> students)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Table Example";

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create an XGraphics object to draw on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Define font and brush
            XFont font = new XFont("Arial", 12);
            XBrush brush = XBrushes.Black;

            // Define the starting position for drawing the table
            double xPos = 50;
            double yPos = 30;

            // Define cell dimensions
            double cellWidth = 150;
            double cellHeight = 30;

            // Column headers (static)
            ///string[] headers = { "Name", "Email", "Amount", "Parent", "No", "Price" };

            string[] headers = { "Name", "Email", "Parent", };

            // Draw the header row
            for (int col = 0; col < headers.Length; col++)
            {
                // Draw a rectangle for each cell in the header
                gfx.DrawRectangle(XPens.Black, xPos + col * cellWidth, yPos, cellWidth, cellHeight);

                // Get the header text
                string cellText = headers[col];

                // Calculate the position to center text in the header cell
                double textWidth = gfx.MeasureString(cellText, font).Width;
                double textHeight = gfx.MeasureString(cellText, font).Height;

                // Center text both horizontally and vertically within the cell
                double xTextPos = xPos + col * cellWidth + (cellWidth - textWidth) / 2;
                double yTextPos = yPos + 10 + (cellHeight - textHeight) / 2;

                // Draw the text inside the header cell
                gfx.DrawString(cellText, font, brush, xTextPos, yTextPos);
            }

            // Move y position down to start drawing the student rows
            yPos += cellHeight;

            // Draw the data rows for each student
            foreach (var student in students)
            {
                // For each student, draw their information in a row
                string[] rowData =
                {
            student.Name,
            student.Email,
          //  student.Amount.ToString(),
            student.Parent,
            // student.No.ToString(),
            // student.Price.ToString("F2")
        };

                for (int col = 0; col < rowData.Length; col++)
                {
                    // Draw a rectangle for each cell in the data row
                    gfx.DrawRectangle(XPens.Black, xPos + col * cellWidth, yPos, cellWidth, cellHeight);

                    // Get the cell text
                    string cellText = rowData[col];

                    // Calculate the position to center text in the cell
                    double textWidth = gfx.MeasureString(cellText, font).Width;
                    double textHeight = gfx.MeasureString(cellText, font).Height;

                    // Center text both horizontally and vertically within the cell
                    double xTextPos = xPos + col * cellWidth + (cellWidth - textWidth) / 2;
                    double yTextPos = yPos + 10 + (cellHeight - textHeight) / 2;

                    // Draw the text inside the cell
                    gfx.DrawString(cellText, font, brush, xTextPos, yTextPos);
                }

                // Move y position down to the next row
                yPos += cellHeight;
            }

            // Save the PDF to a MemoryStream
            using (var pdfStream = new MemoryStream())
            {
                document.Save(pdfStream, false);
                return pdfStream.ToArray();
            }
        }
    }
}