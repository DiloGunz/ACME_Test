using ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;

namespace ACME.CourseManagement.UnitTest.Enrollments.GetByEnrollmentDate;

/// <summary>
/// Pruebas unitarias para GetEnrollmentByEnrollmentDateQueryValidator.
/// </summary>
public class GetEnrollmentByEnrollmentDateQueryValidatorTests
{
    private readonly GetEnrollmentByEnrollmentDateQueryValidator _validator;

    /// <summary>
    /// Inicializa una nueva instancia de las pruebas.
    /// </summary>
    public GetEnrollmentByEnrollmentDateQueryValidatorTests()
    {
        _validator = new GetEnrollmentByEnrollmentDateQueryValidator();
    }

    /// <summary>
    /// Verifica que DateFrom no puede ser mayor o igual a DateTime.UtcNow.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_DateFrom_IsInFutureOrMinValue()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(1), // Fecha en el futuro
            DateTo = DateTime.UtcNow
        };

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.DateFrom);
    }

    /// <summary>
    /// Verifica que DateTo no puede ser menor o igual a DateFrom.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_DateTo_IsBeforeOrEqualToDateFrom()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-1),
            DateTo = DateTime.UtcNow.AddDays(-2) // DateTo antes que DateFrom
        };

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Verifica que DateFrom y DateTo sean válidos cuando están dentro del rango permitido.
    /// </summary>
    [Fact]
    public void Should_NotHaveError_When_DatesAreValid()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-5),
            DateTo = DateTime.UtcNow.AddDays(-1)
        };

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.DateFrom);
        result.ShouldNotHaveValidationErrorFor(x => x.DateTo);
    }
}
