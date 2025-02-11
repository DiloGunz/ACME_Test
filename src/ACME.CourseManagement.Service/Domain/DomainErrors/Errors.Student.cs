namespace ACME.CourseManagement.Service.Domain.DomainErrors;
public static partial class Errors
{
    /// <summary>
    /// Mensajes de error correspondientes a la entidad Student
    /// Para retornar los mensajes se usa ErrorOr.
    /// </summary>
    public static class Student
    {
        /// <summary>
        /// Error cuando el id del estudiante no existe
        /// </summary>
        public static Error IdNotFound =>
            Error.Conflict("Student.Id", "El ID ingresado no existe.");

        /// <summary>
        /// Error cuando el estudiante no es adulto
        /// </summary>
        public static Error AgeNotValid =>
            Error.Validation("Student.Age", "La edad no es válida. Debe ser mayor de 18 años.");

        /// <summary>
        /// Error que muestra cuando el número de documento ingresado ya existe
        /// </summary>
        public static Error AlreadyExistDocumentNumber =>
            Error.Conflict("Student.Documentnumber", "El número de documento ingresado ya existe.");

        /// <summary>
        /// Error cuando el numero de documento ingresado no existe
        /// </summary>
        public static Error NotExistDocumentNumber =>
            Error.NotFound("Student.NotFound", "No se encontró ningun estudiante con el número de documento ingresado.");

        /// <summary>
        /// Error cuando el numero de documento ingresado no existe
        /// </summary>
        public static Error NotRecordsFoundByManyIds =>
            Error.NotFound("Student.FindByManyIds", "No se encontró estudiantes por medio de los Ids ingresados.");
    }
}