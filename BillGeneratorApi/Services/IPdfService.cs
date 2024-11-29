public interface IPdfService
{
    Task<byte[]> GeneratePdfFromExcel(IFormFile file);
}