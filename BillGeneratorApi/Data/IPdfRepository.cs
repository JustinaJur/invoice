namespace billGenerator.Data
{
    public interface IPdfRepository
    {
        IEnumerable<Student> GetAllStudents();
    }
}