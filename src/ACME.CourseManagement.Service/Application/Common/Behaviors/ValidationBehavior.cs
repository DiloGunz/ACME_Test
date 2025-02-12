namespace ACME.CourseManagement.Service.Application.Common.Behaviors;

/// <summary>
/// Comportamiento de validación para Mediator.
/// Se ejecuta antes de que un handler procese una solicitud, validando el request si hay un validador disponible.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud (comando o consulta).</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta esperada, que debe implementar IErrorOr.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Constructor que recibe un validador opcional.
    /// </summary>
    /// <param name="validator">Validador para la solicitud (si existe).</param>
    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    /// <summary>
    /// Ejecuta la validación antes de procesar la solicitud.
    /// </summary>
    /// <param name="request">Solicitud a validar.</param>
    /// <param name="next">Delegado que representa el siguiente paso en la ejecución del pipeline.</param>
    /// <param name="cancellationToken">Token de cancelación de la tarea.</param>
    /// <returns>La respuesta del manejador o una lista de errores de validación.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        // Convierte los errores de validación en una lista de objetos Error.
        var errors = validationResult.Errors.ConvertAll(
            failure => Error.Validation(failure.PropertyName, failure.ErrorMessage));

        // Retorna los errores como una respuesta.
        return (dynamic)errors;
    }
}

