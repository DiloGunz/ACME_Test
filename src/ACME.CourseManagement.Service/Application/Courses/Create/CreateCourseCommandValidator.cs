namespace ACME.CourseManagement.Service.Application.Courses.Create;

/// <summary>
/// Validador para el comando CreateCourseCommand.
/// Utiliza FluentValidation para definir reglas de validación.
/// </summary>
public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    /// <summary>
    /// Constructor que define las reglas de validación para la creación de un curso.
    /// </summary>
    public CreateCourseCommandValidator()
    {
        // El nombre del curso no puede ser nulo ni estar vacío.
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("El nombre del curso es obligatorio.");

        // La tarifa de inscripción debe ser mayor o igual que 0.
        // Si la tarifa de inscripcion es 0, significa que no aplica pago
        RuleFor(x => x.EnrollmentFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("La tarifa de inscripción debe ser mayor o igual que 0.");

        // La fecha de inicio debe ser en el futuro.
        RuleFor(x => x.StartDate)
            .Must(x => x > DateTime.Now)
            .WithMessage("La fecha de inicio debe ser una fecha futura.");

        // La fecha de finalización debe ser en el futuro.
        RuleFor(x => x.EndDate)
            .Must(x => x > DateTime.Now)
            .WithMessage("La fecha de finalización debe ser una fecha futura.");

        // La capacidad debe ser mayor que 0.
        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("La capacidad del curso debe ser mayor que 0.");

        // Validación condicional: si la fecha de inicio es válida, la fecha de finalización debe ser posterior.
        When(x => x.StartDate > DateTime.Now, () =>
        {
            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("La fecha de finalización debe ser posterior a la fecha de inicio.");
        });
    }
}

