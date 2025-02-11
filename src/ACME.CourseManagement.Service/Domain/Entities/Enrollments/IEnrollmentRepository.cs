namespace ACME.CourseManagement.Service.Domain.Entities.Enrollments;

public interface IEnrollmentRepository
{
    Task AddAsync(Enrollment enrollment);
    Task<Enrollment?> GetAsync(long id);
    Task<IReadOnlyList<Enrollment>> GetAllAsync();
    Task<IReadOnlyList<Enrollment>> GetAllByStudentAsync(long studentId);
    Task<IReadOnlyList<Enrollment>> GetAllByCourseAsync(long courseId);
    Task<IReadOnlyList<Enrollment>> GetAllByManyCoursesAsync(List<long> courseIds);
    Task<Enrollment?> GetByStudentAndCourseAsync(long studentId, long courseId);
    Task<List<Enrollment>> GetByEnrollmentDateAsync(DateTime dateFrom, DateTime dateTo);
}