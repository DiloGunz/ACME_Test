namespace ACME.CourseManagement.Service.Domain.Entities.Payments;

/// <summary>
/// Define la interfaz para el repositorio de pagos, proporcionando métodos para gestionar los pagos realizados por los estudiantes.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Agrega un nuevo pago de manera asíncrona.
    /// </summary>
    /// <param name="payment">El pago a registrar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task AddAsync(Payment payment);

    /// <summary>
    /// Obtiene un pago por su identificador único.
    /// </summary>
    /// <param name="id">El identificador del pago.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el pago encontrado o <c>null</c> si no existe.</returns>
    Task<Payment?> GetAsync(long id);

    /// <summary>
    /// Obtiene la lista de todos los pagos registrados.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona con la lista de pagos.</returns>
    Task<IReadOnlyList<Payment>> GetAllAsync();

    /// <summary>
    /// Obtiene el pago realizado por un estudiante en un curso específico.
    /// </summary>
    /// <param name="studentId">El identificador del estudiante.</param>
    /// <param name="courseId">El identificador del curso.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el pago encontrado o <c>null</c> si no existe.</returns>
    Task<Payment?> GetByStudentAndCourseAsync(long studentId, long courseId);
}
