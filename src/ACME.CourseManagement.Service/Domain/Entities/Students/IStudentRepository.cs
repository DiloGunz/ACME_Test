namespace ACME.CourseManagement.Service.Domain.Entities.Students;

/// <summary>
/// Define la interfaz para el repositorio de estudiantes, proporcionando métodos para gestionar la información de los estudiantes.
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Agrega un nuevo estudiante de manera asíncrona.
    /// </summary>
    /// <param name="student">El estudiante a agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task AddAsync(Student student);

    /// <summary>
    /// Obtiene un estudiante por su identificador único.
    /// </summary>
    /// <param name="id">El identificador del estudiante.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el estudiante encontrado o <c>null</c> si no existe.</returns>
    Task<Student?> GetAsync(long id);

    /// <summary>
    /// Obtiene la lista de todos los estudiantes registrados.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de estudiantes.</returns>
    Task<IReadOnlyList<Student>> GetAllAsync();

    /// <summary>
    /// Obtiene un estudiante por su número de documento.
    /// </summary>
    /// <param name="documentNumber">Número de documento del estudiante.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el estudiante encontrado o <c>null</c> si no existe.</returns>
    Task<Student?> GetByDocumentNumberAsync(string documentNumber);

    /// <summary>
    /// Obtiene una lista de estudiantes a partir de múltiples identificadores.
    /// </summary>
    /// <param name="studentsIds">Lista de identificadores de los estudiantes.</param>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de estudiantes encontrados.</returns>
    Task<IReadOnlyList<Student>> FindByManyIdsAsync(List<long> studentsIds);
}
