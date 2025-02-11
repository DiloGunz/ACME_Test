namespace ACME.CourseManagement.Service.Domain.Entities.Students;

public interface IStudentRepository
{
    Task AddAsync(Student student);
    Task<Student?> GetAsync(long id);
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByDocumentNumberAsync(string documentNumber);
    Task<IReadOnlyList<Student>> FindByManyIdsAsync(List<long> studentsIds);
}