namespace billGenerator.Data
{
    public class PdfRepository : IPdfRepository
    {
        public IEnumerable<Student> GetAllStudents()
        {
            return new List<Student>
            {
                new Student {  Name = "Student A", Price = 10.00m },
                new Student {  Name = "Student B", Price = 20.00m },
                new Student { Name = "Student C", Price = 30.00m }
            };
        }

    }
}