using ACME.CourseManagement.Service.Application.Interfaces;

namespace ACME.CourseManagement.Service.Infraestructure.Services;

public class StripePaymentGateway : IPaymentGateway
{
    public async Task<bool> ProcessPayment(long studentId, long courseId, decimal amount)
    {
        Console.WriteLine($"Procesando pago de {amount:C} para el estudiante {studentId} en el curso {courseId} usando Stripe.");
        await Task.Delay(500); // Simula la demora del pago
        return true; // Simula un pago exitoso
    }
}