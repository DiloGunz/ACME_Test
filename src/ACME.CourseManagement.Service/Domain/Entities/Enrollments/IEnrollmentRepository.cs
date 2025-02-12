namespace ACME.CourseManagement.Service.Domain.Entities.Enrollments;

/// <summary>
/// Define la interfaz para el repositorio de inscripciones, proporcionando métodos para gestionar las inscripciones de los estudiantes en cursos.
/// </summary>
public interface IEnrollmentRepository
{
    /// <summary>
    /// Agrega una nueva inscripción de manera asíncrona.
    /// </summary>
    /// <param name="enrollment">La inscripción a agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task AddAsync(Enrollment enrollment);

    /// <summary>
    /// Obtiene una inscripción por su identificador único.
    /// </summary>
    /// <param name="id">El identificador de la inscripción.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la inscripción encontrada o <c>null</c> si no existe.</returns>
    Task<Enrollment?> GetAsync(long id);

    /// <summary>
    /// Obtiene la lista de todas las inscripciones registradas.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de inscripciones.</returns>
    Task<IReadOnlyList<Enrollment>> GetAllAsync();

    /// <summary>
    /// Obtiene todas las inscripciones de un estudiante específico.
    /// </summary>
    /// <param name="studentId">El identificador del estudiante.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de inscripciones del estudiante.</returns>
    Task<IReadOnlyList<Enrollment>> GetAllByStudentAsync(long studentId);

    /// <summary>
    /// Obtiene todas las inscripciones de un curso específico.
    /// </summary>
    /// <param name="courseId">El identificador del curso.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de inscripciones en el curso.</returns>
    Task<IReadOnlyList<Enrollment>> GetAllByCourseAsync(long courseId);

    /// <summary>
    /// Obtiene todas las inscripciones en una lista de cursos específicos.
    /// </summary>
    /// <param name="courseIds">Lista de identificadores de los cursos.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de inscripciones en los cursos especificados.</returns>
    Task<IReadOnlyList<Enrollment>> GetAllByManyCoursesAsync(List<long> courseIds);

    /// <summary>
    /// Obtiene una inscripción específica de un estudiante en un curso determinado.
    /// </summary>
    /// <param name="studentId">El identificador del estudiante.</param>
    /// <param name="courseId">El identificador del curso.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la inscripción encontrada o <c>null</c> si no existe.</returns>
    Task<Enrollment?> GetByStudentAndCourseAsync(long studentId, long courseId);

    /// <summary>
    /// Obtiene todas las inscripciones realizadas dentro de un rango de fechas.
    /// </summary>
    /// <param name="dateFrom">Fecha de inicio del rango.</param>
    /// <param name="dateTo">Fecha de fin del rango.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de inscripciones dentro del rango de fechas.</returns>
    Task<List<Enrollment>> GetByEnrollmentDateAsync(DateTime dateFrom, DateTime dateTo);
}
