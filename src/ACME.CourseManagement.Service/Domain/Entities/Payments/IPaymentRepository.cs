namespace ACME.CourseManagement.Service.Domain.Entities.Payments;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
    Task<Payment?> GetAsync(long id);
    Task<IReadOnlyList<Payment>> GetAllAsync();
    Task<Payment?> GetByStudentAndCourseAsync(long studentId, long courseId);
}