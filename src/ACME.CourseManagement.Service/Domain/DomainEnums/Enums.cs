namespace ACME.CourseManagement.Service.Domain.DomainEnums;

public class Enums
{
    public enum PaymentStatus
    {
        Pending,    // Pago pendiente
        Completed,  // Pago completado
        Failed,     // Pago fallido
        Canceled,   // Pago cancelado
        Refunded,   // Pago reembolsado
        Processing  // Pago en proceso
    }

}