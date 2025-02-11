namespace ACME.CourseManagement.Service.Application.Courses.GetById;

/// <summary>
/// Validador para la consulta GetCourseByIdQuery.
/// Utiliza FluentValidation para definir reglas de validación.
/// </summary>
public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    /// <summary>
    /// Constructor que define las reglas de validación.
    /// </summary>
    public GetCourseByIdQueryValidator()
    {
        // Regla: El Id debe ser mayor que 0.
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("El Id del curso debe ser un número mayor que 0.");
    }
}