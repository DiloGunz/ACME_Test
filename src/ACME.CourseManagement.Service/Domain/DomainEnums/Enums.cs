namespace ACME.CourseManagement.Service.Domain.DomainEnums;

/// <summary>
/// Contiene enumeraciones utilizadas en el sistema.
/// </summary>
public class Enums
{
    /// <summary>
    /// Representa los diferentes estados de un pago.
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// El pago ha sido registrado, pero aún no se ha procesado.
        /// </summary>
        Pending,

        /// <summary>
        /// El pago se ha completado exitosamente.
        /// </summary>
        Completed,

        /// <summary>
        /// El pago ha fallado durante el proceso.
        /// </summary>
        Failed,

        /// <summary>
        /// El pago ha sido cancelado por el usuario o el sistema.
        /// </summary>
        Canceled,

        /// <summary>
        /// El pago ha sido reembolsado al usuario.
        /// </summary>
        Refunded,

        /// <summary>
        /// El pago está en proceso y aún no se ha completado.
        /// </summary>
        Processing
    }
}
