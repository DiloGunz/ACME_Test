namespace ACME.CourseManagement.Service.Domain.Entities.Courses;

/// <summary>
/// Define la interfaz para el repositorio de cursos, proporcionando métodos para gestionar cursos en la base de datos.
/// </summary>
public interface ICourseRepository
{
    /// <summary>
    /// Agrega un nuevo curso de manera asíncrona.
    /// </summary>
    /// <param name="course">El curso a agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task AddAsync(Course course);

    /// <summary>
    /// Verifica si un curso con el mismo nombre ya existe.
    /// </summary>
    /// <param name="courseName">El nombre del curso a verificar.</param>
    /// <returns>Retorna <c>true</c> si el nombre ya existe; de lo contrario, <c>false</c>.</returns>
    Task<bool> ExistsNameAsync(string courseName);

    /// <summary>
    /// Obtiene un curso por su identificador único.
    /// </summary>
    /// <param name="id">El identificador del curso.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el curso encontrado o <c>null</c> si no existe.</returns>
    Task<Course?> GetAsync(long id);

    /// <summary>
    /// Obtiene la lista de todos los cursos disponibles.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de cursos.</returns>
    Task<IReadOnlyList<Course>> GetAllAsync();

    /// <summary>
    /// Obtiene una lista de cursos según una lista de identificadores.
    /// </summary>
    /// <param name="courseIds">Lista de identificadores de los cursos a obtener.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de cursos encontrados.</returns>
    Task<IReadOnlyList<Course>> GetByManyidsAsync(List<long> courseIds);

    /// <summary>
    /// Busca cursos dentro de un rango de fechas de inicio.
    /// </summary>
    /// <param name="dateFrom">Fecha de inicio del rango.</param>
    /// <param name="dateTo">Fecha de fin del rango.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de cursos encontrados dentro del rango de fechas.</returns>
    Task<IReadOnlyList<Course>> FindByDateRangeAsync(DateTime dateFrom, DateTime dateTo);
}
