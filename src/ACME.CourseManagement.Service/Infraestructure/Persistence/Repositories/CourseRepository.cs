using ACME.CourseManagement.Service.Domain.Entities.Courses;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = new();

    public Task AddAsync(Course course)
    {
        _courses.Add(course);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsNameAsync(string courseName)
    {
        return Task.FromResult(_courses.Any(c => c.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<IReadOnlyList<Course>> FindByDateRangeAsync(DateTime dateFrom, DateTime dateTo)
    {
        var result = _courses.Where(c => c.StartDate >= dateFrom && c.StartDate <= dateTo).ToList();
        return Task.FromResult<IReadOnlyList<Course>>(result);
    }

    public Task<IReadOnlyList<Course>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Course>>(_courses);
    }

    public Task<Course?> GetAsync(long id)
    {
        return Task.FromResult(_courses.FirstOrDefault(c => c.Id == id));
    }

    public Task<IReadOnlyList<Course>> GetByManyidsAsync(List<long> courseIds)
    {
        var result = _courses.Where(c => courseIds.Contains(c.Id)).ToList();
        return Task.FromResult<IReadOnlyList<Course>>(result);
    }
}