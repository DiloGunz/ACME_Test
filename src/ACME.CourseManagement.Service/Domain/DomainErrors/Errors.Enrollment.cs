namespace ACME.CourseManagement.Service.Domain.DomainErrors;

public static partial class Errors
{
    public static class Enrollment
    {
        public static Error IdStudentAndIsCourseNotFound =>
            Error.Conflict("Enrollment", "No existe incripción de alumno y curso.");

        public static Error StudentHasAlreadyBeenEnrolled =>
            Error.Conflict("Enrollement.Enrolled", "El estudiante ya se ha inscrito en este curso previamente.");

        public static Error EnrollmentDateNotFound =>
            Error.Failure("Enrollement.Ids", "No se encontraron registros en el rango de fechas ingresado.");
    }
}