namespace ACME.CourseManagement.Service.Application.Courses.Create;

/// <summary>
/// Representa un comando para la creación de un curso.
/// Implementa IRequest con un resultado de tipo ErrorOr<long>.
/// </summary>
public record CreateCourseCommand : IRequest<ErrorOr<long>>
{
    /// <summary>
    /// Nombre del curso.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Costo de inscripción del curso.
    /// Si el costo de inscripción es 0, significa que no aplica el pago
    /// </summary>
    public decimal EnrollmentFee { get; set; }

    /// <summary>
    /// Capacidad máxima de estudiantes permitidos en el curso.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Fecha de inicio del curso.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del curso.
    /// </summary>
    public DateTime EndDate { get; set; }
}
