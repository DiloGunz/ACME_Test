namespace ACME.CourseManagement.Service.Application.Students.Common;

/// <summary>
/// Representa un objeto de transferencia de datos (DTO) para un estudiante.
/// </summary>
public record StudentDto
{
    /// <summary>
    /// Identificador único del estudiante.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Nombre del estudiante.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Apellido del estudiante.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Número de documento del estudiante (DNI, pasaporte, etc.).
    /// </summary>
    public string DocumentNumber { get; set; }

    /// <summary>
    /// Edad del estudiante.
    /// </summary>
    public int Age { get; set; }
}

