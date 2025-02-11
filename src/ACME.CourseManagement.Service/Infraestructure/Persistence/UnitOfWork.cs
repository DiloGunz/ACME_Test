using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private int _saveChangesReturnValue;

    public UnitOfWork(int saveChangesReturnValue = 1)
    {
        _saveChangesReturnValue = saveChangesReturnValue;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_saveChangesReturnValue);
    }
}