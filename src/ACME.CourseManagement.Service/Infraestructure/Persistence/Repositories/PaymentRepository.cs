using ACME.CourseManagement.Service.Domain.Entities.Payments;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments = new();

    /// <summary>
    /// Agrega un nuevo pago a la lista simulada.
    /// </summary>
    public Task AddAsync(Payment payment)
    {
        _payments.Add(payment);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Obtiene todos los pagos registrados en la lista simulada.
    /// </summary>
    public Task<IReadOnlyList<Payment>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Payment>>(_payments);
    }

    /// <summary>
    /// Obtiene un pago específico por su ID.
    /// </summary>
    public Task<Payment?> GetAsync(long id)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(payment);
    }

    /// <summary>
    /// Obtiene un pago específico realizado por un estudiante en un curso determinado.
    /// </summary>
    public Task<Payment?> GetByStudentAndCourseAsync(long studentId, long courseId)
    {
        var payment = _payments.FirstOrDefault(p => p.StudentId == studentId && p.CourseId == courseId);
        return Task.FromResult(payment);
    }
}