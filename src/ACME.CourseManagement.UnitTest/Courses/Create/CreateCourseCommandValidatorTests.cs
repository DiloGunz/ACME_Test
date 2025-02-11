using ACME.CourseManagement.Service.Application.Courses.Create;

namespace ACME.CourseManagement.UnitTest.Courses.Create;

public class CreateCourseCommandValidatorTests
{
    private readonly CreateCourseCommandValidator _validator;

    public CreateCourseCommandValidatorTests()
    {
        _validator = new CreateCourseCommandValidator();
    }

    /// <summary>
    /// Valida que se genere un error cuando el nombre del curso es nulo o vacío.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_Name_Is_Null_Or_Empty(string name)
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 100,
            Name = name,
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Valida que se genere un error cuando la tarifa de inscripción es negativa.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_EnrollmentFeeIsNegative()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = -1,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EnrollmentFee);
    }

    /// <summary>
    /// Valida que se genere un error cuando la fecha de inicio no es futura.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_StartDateIsNotInFuture()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 100,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    /// <summary>
    /// Valida que se genere un error cuando la fecha de finalización no es futura.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_EndDateIsNotInFuture()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 100,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    /// <summary>
    /// Valida que se genere un error cuando la capacidad del curso es menor o igual a 0.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Capacity_Is_Zero_Or_Negative()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 0,
            EnrollmentFee = 100,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Capacity);
    }

    /// <summary>
    /// Valida que se genere un error cuando la fecha de finalización es anterior a la fecha de inicio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Before_Start_Date()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 100,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Valida que no haya errores cuando todos los valores son correctos.
    /// </summary>
    [Fact]
    public void Should_NotHaveError_When_AllValuesAreValid()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 100,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.EnrollmentFee);
        result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
        result.ShouldNotHaveValidationErrorFor(x => x.EndDate);
        result.ShouldNotHaveValidationErrorFor(x => x.Capacity);
    }
}