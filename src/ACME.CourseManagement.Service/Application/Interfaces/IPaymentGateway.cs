namespace ACME.CourseManagement.Service.Application.Interfaces;

/// <summary>
/// Define la interfaz para una pasarela de pago, proporcionando métodos para procesar pagos.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Procesa un pago para un estudiante en un curso específico.
    /// </summary>
    /// <param name="studentId">El identificador del estudiante que realiza el pago.</param>
    /// <param name="courseId">El identificador del curso al que se asocia el pago.</param>
    /// <param name="amount">El monto del pago a procesar.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve <c>true</c> si el pago fue exitoso, de lo contrario <c>false</c>.</returns>
    Task<bool> ProcessPayment(long studentId, long courseId, decimal amount);
}
