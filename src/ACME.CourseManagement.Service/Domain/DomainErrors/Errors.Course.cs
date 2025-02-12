namespace ACME.CourseManagement.Service.Domain.DomainErrors;

/// <summary>
/// Contiene definiciones de errores específicos para distintas entidades del sistema.
/// </summary>
public static partial class Errors
{
    /// <summary>
    /// Contiene errores relacionados con la entidad <see cref="Course"/>.
    /// </summary>
    public static class Course
    {
        /// <summary>
        /// Error que indica que el ID del curso no fue encontrado.
        /// </summary>
        public static Error IdNotFound =>
            Error.Conflict("Course.Id", "El ID ingresado no existe.");

        /// <summary>
        /// Error que indica que el curso está deshabilitado y no permite inscripciones.
        /// </summary>
        public static Error Disable =>
            Error.Conflict("Course.Enable", "No se puede inscribir en un curso que no está disponible.");

        /// <summary>
        /// Error que indica que no hay vacantes disponibles en el curso.
        /// </summary>
        public static Error InsufficientVacancies =>
            Error.Conflict("Course.Capacity", "El curso no tiene vacantes disponibles.");

        /// <summary>
        /// Error que indica que no se encontraron registros en el rango de fechas especificado.
        /// </summary>
        public static Error NotRecordsFoundInDateRange =>
            Error.Conflict("Course.FindByDateRanges", "No se encontraron registros en el rango de fechas ingresados.");

        /// <summary>
        /// Error que indica que el nombre del curso ya existe.
        /// </summary>
        public static Error NameAlreadyExists =>
            Error.Conflict("Course.Name", "El nombre ingresado ya existe.");
    }
}
