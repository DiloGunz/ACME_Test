using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Courses;


/// <summary>
/// Representa un curso.
/// </summary>
public class Course
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
    /// Tarifa de inscripción al curso.
    /// </summary>
    public decimal EnrollmentFee { get; set; }

    /// <summary>
    /// Capacidad máxima de estudiantes permitidos en el curso.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Indica si el curso está habilitado o no.
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

    /// <summary>
    /// Constructor por defecto requerido por ciertos frameworks y herramientas de serialización.
    /// </summary>
    public Course()
    {
    }

    /// <summary>
    /// Constructor para crear un curso con detalles específicos.
    /// </summary>
    /// <param name="name">Nombre del curso.</param>
    /// <param name="enrollmentFee">Tarifa de inscripción del curso.</param>
    /// <param name="capacity">Capacidad máxima del curso.</param>
    /// <param name="startDate">Fecha de inicio del curso.</param>
    /// <param name="endDate">Fecha de finalización del curso.</param>
    public Course(string name, decimal enrollmentFee, int capacity, DateTime startDate, DateTime endDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        Name = name;
        EnrollmentFee = enrollmentFee;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Enable = true;
    }
}