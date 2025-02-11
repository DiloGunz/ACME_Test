namespace ACME.CourseManagement.Service.Application.Courses.Common;

/// <summary>
/// Representa un objeto de transferencia de datos (DTO) para un curso.
/// </summary>
public record CourseDto
{
    /// <summary>
    /// Identificador único del curso.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Nombre del curso.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Costo de inscripción del curso.
    /// </summary>
    public decimal EnrollmentFee { get; set; }

    /// <summary>
    /// Capacidad máxima de estudiantes permitidos en el curso.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Indica si el curso está habilitado.
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Fecha de inicio del curso.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Fecha de finalización del curso.
    /// </summary>
    public DateTime EndDate { get; set; }
}
