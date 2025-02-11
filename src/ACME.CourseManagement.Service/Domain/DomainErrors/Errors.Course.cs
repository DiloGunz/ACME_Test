namespace ACME.CourseManagement.Service.Domain.DomainErrors;

public static partial class Errors
{
    /// <summary>
    /// Mensajes de error correspondientes a la entidad Course
    /// Para retornar los mensajes se usa ErrorOr.
    /// </summary>
    public static class Course
    {
        /// <summary>
        /// Error cuando el id del sourse no existe
        /// </summary>
        public static Error IdNotFound =>
            Error.Conflict("Course.Id", "El ID ingresado no existe.");

        public static Error Disable =>
            Error.Conflict("Course.Enable", "No se puede inscribir en un curso que no está disponible.");

        public static Error InsufficientVacancies =>
            Error.Conflict("Course.Capacity", "El curso no tiene vacantes disponibles");

        public static Error NotRecordsFoundInDateRange =>
            Error.Conflict("Course.FindByDateRanges", "No se encontraron registros en el rango de fechas ingresados");

        public static Error NameAlreadyExists =>
            Error.Conflict("Course.Name", "El nombre ingresado ya existe.");
    }
}