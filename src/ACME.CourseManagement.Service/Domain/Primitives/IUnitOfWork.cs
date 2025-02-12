namespace ACME.CourseManagement.Service.Domain.Primitives;

/// <summary>
/// Define la interfaz para la unidad de trabajo (Unit of Work), que coordina la persistencia de cambios en el sistema.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Guarda los cambios pendientes en la base de datos de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token opcional para cancelar la operación.</param>
    /// <returns>El número de entidades afectadas por la operación.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
