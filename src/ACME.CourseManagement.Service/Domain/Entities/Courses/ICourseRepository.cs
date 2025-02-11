namespace ACME.CourseManagement.Service.Domain.Entities.Courses;

public interface ICourseRepository
{
    Task AddAsync(Course course);
    /// <summary>
    /// Retorna true si el nombre ya existe
    /// </summary>
    /// <param name="courseName"></param>
    /// <returns></returns>
    Task<bool> ExistsNameAsync(string courseName);
    Task<Course?> GetAsync(long id);
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task<IReadOnlyList<Course>> GetByManyidsAsync(List<long> courseIds);
    /// <summary>
    /// Esta funcion busca por la fecha de inicio del curso
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Course>> FindByDateRangeAsync(DateTime dateFrom, DateTime dateTo);
}