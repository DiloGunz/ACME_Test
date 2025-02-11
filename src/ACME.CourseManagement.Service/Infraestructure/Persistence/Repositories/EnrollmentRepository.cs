using ACME.CourseManagement.Service.Domain.Entities.Enrollments;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly List<Enrollment> _enrollments = new();

    /// <summary>
    /// Agrega una nueva inscripción a la lista.
    /// </summary>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    public Task AddAsync(Enrollment enrollment)
    {
        _enrollments.Add(enrollment);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Obtiene todas las inscripciones disponibles.
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyList<Enrollment>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Enrollment>>(_enrollments);
    }

    /// <summary>
    /// Obtiene todas las inscripciones de un curso específico.
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<Enrollment>> GetAllByCourseAsync(long courseId)
    {
        var result = _enrollments.Where(e => e.CourseId == courseId).ToList();
        return Task.FromResult<IReadOnlyList<Enrollment>>(result);
    }

    /// <summary>
    /// Obtiene todas las inscripciones de varios cursos dados sus identificadores.
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<Enrollment>> GetAllByManyCoursesAsync(List<long> courseIds)
    {
        var result = _enrollments.Where(e => courseIds.Contains(e.CourseId)).ToList();
        return Task.FromResult<IReadOnlyList<Enrollment>>(result);
    }

    /// <summary>
    /// Obtiene todas las inscripciones de un estudiante específico.
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<Enrollment>> GetAllByStudentAsync(long studentId)
    {
        var result = _enrollments.Where(e => e.StudentId == studentId).ToList();
        return Task.FromResult<IReadOnlyList<Enrollment>>(result);
    }

    /// <summary>
    /// Obtiene una inscripción específica por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Enrollment?> GetAsync(long id)
    {
        var enrollment = _enrollments.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(enrollment);
    }

    /// <summary>
    /// Obtiene las incripciones ehchas en un rango de fechas
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<Enrollment>> GetByEnrollmentDateAsync(DateTime dateFrom, DateTime dateTo)
    {
        var enrollments = _enrollments.Where(e => e.EnrollmentDate >= dateFrom && e.EnrollmentDate <= dateTo).ToList();
        return Task.FromResult(enrollments);
    }

    /// <summary>
    /// Obtiene una inscripción de un estudiante en un curso específico.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    public Task<Enrollment?> GetByStudentAndCourseAsync(long studentId, long courseId)
    {
        var enrollment = _enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);
        return Task.FromResult(enrollment);
    }
}