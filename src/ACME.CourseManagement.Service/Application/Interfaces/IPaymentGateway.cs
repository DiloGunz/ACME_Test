namespace ACME.CourseManagement.Service.Application.Interfaces;

public interface IPaymentGateway
{
    Task<bool> ProcessPayment(long studentId, long courseId, decimal amount);
}