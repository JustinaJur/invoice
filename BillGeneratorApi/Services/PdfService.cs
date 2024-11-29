namespace billGenerator.Services
{
    using PdfSharpCore.Pdf;
    using PdfSharpCore.Drawing;
    using System.Collections.Generic;
    using System.IO;
    using OfficeOpenXml;
    using System;
    using billGenerator.Helpers;

    public class PDFService
    {
        public class PdfService : IPdfService
        {
            public async Task<byte[]> GeneratePdfFromExcel(IFormFile file)
            {
                var students = await ExcelHelper.ProcessExcelFile(file);
                var pdfBytes = ExcelHelper.GeneratePdf(students);

                return pdfBytes;
            }
        }
    }
}