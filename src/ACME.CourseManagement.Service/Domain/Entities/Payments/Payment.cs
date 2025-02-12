using ACME.CourseManagement.Service.Domain.DomainEnums;
using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Payments;

/// <summary>
/// Representa un pago realizado por un estudiante para un curso.
/// </summary>
public class Payment
{
    /// <summary>
    /// Identificador único del pago.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador del estudiante que realiza el pago.
    /// </summary>
    public long StudentId { get; set; }

    /// <summary>
    /// Identificador del curso al que se asocia el pago.
    /// </summary>
    public long CourseId { get; set; }

    /// <summary>
    /// Monto pagado por el estudiante.
    /// </summary>
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Estado actual del pago (Ejemplo: Pendiente, Completado, Rechazado).
    /// </summary>
    public Enums.PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Fecha en la que se realizó el pago.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Constructor por defecto requerido por ciertos frameworks y herramientas de serialización.
    /// </summary>
    public Payment()
    {
    }

    /// <summary>
    /// Constructor para registrar un pago con detalles específicos.
    /// </summary>
    /// <param name="studentId">Identificador del estudiante que realiza el pago.</param>
    /// <param name="courseId">Identificador del curso asociado al pago.</param>
    /// <param name="amountPaid">Monto del pago realizado.</param>
    /// <param name="paymentStatus">Estado del pago.</param>
    /// <param name="paymentDate">Fecha en la que se realizó el pago.</param>
    public Payment(long studentId, long courseId, decimal amountPaid, Enums.PaymentStatus paymentStatus, DateTime paymentDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        StudentId = studentId;
        CourseId = courseId;
        AmountPaid = amountPaid;
        PaymentStatus = paymentStatus;
        PaymentDate = paymentDate;
    }
}
