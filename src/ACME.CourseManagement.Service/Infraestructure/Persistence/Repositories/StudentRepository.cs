using ACME.CourseManagement.Service.Domain.Entities.Students;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new List<Student>();

    public Task AddAsync(Student student)
    {
        _students.Add(student);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Student>> FindByManyIdsAsync(List<long> studentsIds)
    {
        IReadOnlyList<Student> readOnlyStudents = _students.Where(x => studentsIds.Contains(x.Id)).ToList().AsReadOnly();
        return Task.FromResult(readOnlyStudents);
    }

    public Task<IReadOnlyList<Student>> GetAllAsync()
    {
        IReadOnlyList<Student> readOnlyStudents = _students.AsReadOnly();
        return Task.FromResult(readOnlyStudents);
    }

    public Task<Student?> GetAsync(long id) 
    {
        var result = _students.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(result);
    }

    public Task<Student?> GetByDocumentNumberAsync(string documentNumber)
    {
        var result = _students.SingleOrDefault(x => x.DocumentNumber == documentNumber);
        return Task.FromResult(result);
    }
}