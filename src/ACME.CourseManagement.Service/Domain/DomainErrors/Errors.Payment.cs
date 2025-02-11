namespace ACME.CourseManagement.Service.Domain.DomainErrors;

public static partial class Errors
{
    /// <summary>
    /// Mensajes de error correspondientes a la entidad Course
    /// Para retornar los mensajes se usa ErrorOr.
    /// </summary>
    public static class Payment
    {
        public static Error ProcessFailure =>
           Error.Failure("Payment", "Error en el procesamiento del pago.");

        public static Error PaymentCannotProceedForZeroAmount =>
           Error.Failure("Payment.Ammount", "El pago no puede proceder por monto 0.");

        public static Error IdStudentAndIdCoursePaymentNotFound =>
           Error.Failure("Payment.Id", "No se encontró pago asociado al estudiante y curso ingresados.");
    }
}