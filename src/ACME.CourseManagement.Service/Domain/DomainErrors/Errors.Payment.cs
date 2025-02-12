namespace ACME.CourseManagement.Service.Domain.DomainErrors;

/// <summary>
/// Contiene definiciones de errores específicos para distintas entidades del sistema.
/// </summary>
public static partial class Errors
{
    /// <summary>
    /// Contiene errores relacionados con la entidad <see cref="Payment"/>.
    /// </summary>
    public static class Payment
    {
        /// <summary>
        /// Error que indica que ocurrió un problema durante el procesamiento del pago.
        /// </summary>
        public static Error ProcessFailure =>
           Error.Failure("Payment", "Error en el procesamiento del pago.");

        /// <summary>
        /// Error que indica que el pago no puede proceder debido a que el monto es 0.
        /// </summary>
        public static Error PaymentCannotProceedForZeroAmount =>
           Error.Failure("Payment.Amount", "El pago no puede proceder por monto 0.");

        /// <summary>
        /// Error que indica que no se encontró un pago asociado al estudiante y curso ingresados.
        /// </summary>
        public static Error IdStudentAndIdCoursePaymentNotFound =>
           Error.Failure("Payment.Id", "No se encontró pago asociado al estudiante y curso ingresados.");
    }
}
