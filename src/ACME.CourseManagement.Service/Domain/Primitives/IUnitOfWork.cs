namespace ACME.CourseManagement.Service.Domain.Primitives;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}