using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Enrollments;

/// <summary>
/// Representa la inscripción de un estudiante en un curso.
/// </summary>
public class Enrollment
{
    /// <summary>
    /// Identificador único de la inscripción.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador del estudiante que se inscribe en el curso.
    /// </summary>
    public long StudentId { get; set; }

    /// <summary>
    /// Identificador del curso en el que el estudiante está inscrito.
    /// </summary>
    public long CourseId { get; set; }

    /// <summary>
    /// Fecha en la que se realizó la inscripción.
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Constructor por defecto requerido por ciertos frameworks y herramientas de serialización.
    /// </summary>
    public Enrollment()
    {
    }

    /// <summary>
    /// Constructor para crear una inscripción con detalles específicos.
    /// </summary>
    /// <param name="studentId">Identificador del estudiante.</param>
    /// <param name="courseId">Identificador del curso.</param>
    /// <param name="enrollmentDate">Fecha en la que se realizó la inscripción.</param>
    public Enrollment(long studentId, long courseId, DateTime enrollmentDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        StudentId = studentId;
        CourseId = courseId;
        EnrollmentDate = enrollmentDate;
    }
}
