using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Infraestructure.Persistence;

/// <summary>
/// Implementación de la unidad de trabajo (Unit of Work) que gestiona la persistencia de cambios en la base de datos.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Valor de retorno simulado para la operación de guardado de cambios.
    /// </summary>
    private int _saveChangesReturnValue;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UnitOfWork"/> con un valor predeterminado para el número de cambios guardados.
    /// </summary>
    /// <param name="saveChangesReturnValue">
    /// Valor que se retornará cuando se invoque <see cref="SaveChangesAsync"/>.
    /// Por defecto, devuelve 1, lo que simula una operación exitosa.
    /// </param>
    public UnitOfWork(int saveChangesReturnValue = 1)
    {
        _saveChangesReturnValue = saveChangesReturnValue;
    }

    /// <summary>
    /// Guarda los cambios pendientes en la base de datos de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token opcional para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve el número de cambios guardados.</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_saveChangesReturnValue);
    }
}
