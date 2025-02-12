namespace ACME.CourseManagement.Service.Domain.DomainErrors;

/// <summary>
/// Contiene definiciones de errores específicos para distintas entidades del sistema.
/// </summary>
public static partial class Errors
{
    /// <summary>
    /// Contiene errores relacionados con la entidad <see cref="Enrollment"/>.
    /// </summary>
    public static class Enrollment
    {
        /// <summary>
        /// Error que indica que no existe una inscripción asociada al estudiante y al curso proporcionados.
        /// </summary>
        public static Error IdStudentAndIsCourseNotFound =>
            Error.Conflict("Enrollment", "No existe inscripción de alumno y curso.");

        /// <summary>
        /// Error que indica que el estudiante ya está inscrito en el curso.
        /// </summary>
        public static Error StudentHasAlreadyBeenEnrolled =>
            Error.Conflict("Enrollment.Enrolled", "El estudiante ya se ha inscrito en este curso previamente.");

        /// <summary>
        /// Error que indica que no se encontraron registros de inscripciones dentro del rango de fechas especificado.
        /// </summary>
        public static Error EnrollmentDateNotFound =>
            Error.Failure("Enrollment.Ids", "No se encontraron registros en el rango de fechas ingresado.");
    }
}
