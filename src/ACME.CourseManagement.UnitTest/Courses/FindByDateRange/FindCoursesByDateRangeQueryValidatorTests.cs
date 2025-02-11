using ACME.CourseManagement.Service.Application.Courses.FindByDateRange;

namespace ACME.CourseManagement.UnitTest.Courses.FindByDateRange;

public class FindCoursesByDateRangeQueryValidatorTests
{
    private readonly FindCoursesByDateRangeQueryValidator _validator;

    public FindCoursesByDateRangeQueryValidatorTests()
    {
        _validator = new FindCoursesByDateRangeQueryValidator();
    }

    /// <summary>
    /// Valida que se genere un error cuando DateFrom es mayor o igual a la fecha actual.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_DateFrom_Is_In_The_Future_Or_Now()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.UtcNow.AddDays(1), DateTo = DateTime.UtcNow.AddDays(10) };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.DateFrom);
    }

    /// <summary>
    /// Valida que se genere un error cuando DateFrom es menor que DateTime.MinValue.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_DateFrom_Is_MinValue()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.MinValue, DateTo = DateTime.UtcNow.AddDays(-10) };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.DateFrom);
    }

    /// <summary>
    /// Valida que se genere un error cuando DateTo es mayor o igual a la fecha actual.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_DateTo_Is_In_The_Future_Or_Now()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.UtcNow.AddDays(-10), DateTo = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.DateTo);
    }

    /// <summary>
    /// Valida que se genere un error cuando DateTo es menor que DateTime.MinValue.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Date_To_Is_Min_Value()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.UtcNow.AddDays(-10), DateTo = DateTime.MinValue };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.DateTo);
    }

    /// <summary>
    /// Valida que se genere un error cuando DateTo es menor o igual a DateFrom.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Date_To_Is_Before_Or_Equal_To_Date_From()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.UtcNow.AddDays(-10), DateTo = DateTime.UtcNow.AddDays(-11) };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Valida que no haya errores cuando DateFrom y DateTo son válidos.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_Date_Range_Is_Valid()
    {
        var query = new FindCoursesByDateRangeQuery { DateFrom = DateTime.UtcNow.AddDays(-30), DateTo = DateTime.UtcNow.AddDays(-10) };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.DateFrom);
        result.ShouldNotHaveValidationErrorFor(x => x.DateTo);
    }
}