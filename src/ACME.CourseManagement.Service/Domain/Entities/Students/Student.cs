using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Students;

/// <summary>
/// Representa a un estudiante con información personal y de identificación.
/// </summary>
public class Student
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
    /// Número de documento de identidad del estudiante.
    /// </summary>
    public string DocumentNumber { get; set; }

    /// <summary>
    /// Edad del estudiante.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Constructor por defecto requerido por ciertos frameworks y herramientas de serialización.
    /// </summary>
    public Student()
    {
    }

    /// <summary>
    /// Constructor para crear un estudiante con detalles específicos.
    /// </summary>
    /// <param name="firstName">Nombre del estudiante.</param>
    /// <param name="lastName">Apellido del estudiante.</param>
    /// <param name="age">Edad del estudiante.</param>
    /// <param name="documentNumber">Número de documento del estudiante.</param>
    public Student(string firstName, string lastName, int age, string documentNumber)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        DocumentNumber = documentNumber;
    }
}
