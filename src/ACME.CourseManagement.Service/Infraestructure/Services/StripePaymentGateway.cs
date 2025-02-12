using ACME.CourseManagement.Service.Application.Interfaces;

namespace ACME.CourseManagement.Service.Infraestructure.Services;

/// <summary>
/// Implementación de la pasarela de pago utilizando el servicio de Stripe.
/// </summary>
public class StripePaymentGateway : IPaymentGateway
{
    /// <summary>
    /// Procesa un pago para un estudiante en un curso específico utilizando Stripe.
    /// </summary>
    /// <param name="studentId">El identificador del estudiante que realiza el pago.</param>
    /// <param name="courseId">El identificador del curso al que se asocia el pago.</param>
    /// <param name="amount">El monto del pago a procesar.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve <c>true</c> si el pago fue exitoso, de lo contrario <c>false</c>.</returns>
    public async Task<bool> ProcessPayment(long studentId, long courseId, decimal amount)
    {
        Console.WriteLine($"Procesando pago de {amount:C} para el estudiante {studentId} en el curso {courseId} usando Stripe.");

        // Simula la demora del procesamiento del pago
        await Task.Delay(1000);

        return true; // Simula un pago exitoso
    }
}