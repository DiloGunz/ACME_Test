namespace ACME.CourseManagement.Service.Domain.DomainErrors;

/// <summary>
/// Contiene definiciones de errores específicos para distintas entidades del sistema.
/// </summary>
public static partial class Errors
{
    /// <summary>
    /// Contiene errores relacionados con la entidad <see cref="Student"/>.
    /// Para retornar los mensajes se usa <c>ErrorOr</c>.
    /// </summary>
    public static class Student
    {
        /// <summary>
        /// Error que indica que el ID del estudiante no existe.
        /// </summary>
        public static Error IdNotFound =>
            Error.Conflict("Student.Id", "El ID ingresado no existe.");

        /// <summary>
        /// Error que indica que la edad del estudiante no es válida (debe ser mayor de 18 años).
        /// </summary>
        public static Error AgeNotValid =>
            Error.Validation("Student.Age", "La edad no es válida. Debe ser mayor de 18 años.");

        /// <summary>
        /// Error que indica que el número de documento ya existe en el sistema.
        /// </summary>
        public static Error AlreadyExistDocumentNumber =>
            Error.Conflict("Student.DocumentNumber", "El número de documento ingresado ya existe.");

        /// <summary>
        /// Error que indica que no se encontró ningún estudiante con el número de documento ingresado.
        /// </summary>
        public static Error NotExistDocumentNumber =>
            Error.NotFound("Student.NotFound", "No se encontró ningún estudiante con el número de documento ingresado.");

        /// <summary>
        /// Error que indica que no se encontraron estudiantes con los identificadores proporcionados.
        /// </summary>
        public static Error NotRecordsFoundByManyIds =>
            Error.NotFound("Student.FindByManyIds", "No se encontraron estudiantes con los identificadores ingresados.");
    }
}
