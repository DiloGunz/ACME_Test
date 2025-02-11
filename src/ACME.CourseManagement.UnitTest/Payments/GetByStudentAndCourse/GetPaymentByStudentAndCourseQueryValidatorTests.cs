using ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;

namespace ACME.CourseManagement.UnitTest.Payments.GetByStudentAndCourse;

public class GetPaymentByStudentAndCourseQueryValidatorTests
{
    private readonly GetPaymentByStudentAndCourseQueryValidator _validator;

    public GetPaymentByStudentAndCourseQueryValidatorTests()
    {
        _validator = new GetPaymentByStudentAndCourseQueryValidator();
    }

    /// <summary>
    /// Valida que se genere un error cuando StudentId es igual a 0.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_StudentId_Is_Zero()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 0, CourseId = 1 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.StudentId);
    }

    /// <summary>
    /// Valida que se genere un error cuando StudentId es un número negativo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_StudentId_Is_Negative()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = -1, CourseId = 1 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.StudentId);
    }

    /// <summary>
    /// Valida que se genere un error cuando CourseId es igual a 0.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_CourseId_Is_Zero()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 1, CourseId = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    /// <summary>
    /// Valida que se genere un error cuando CourseId es un número negativo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_CourseId_Is_Negative()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 1, CourseId = -1 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    /// <summary>
    /// Valida que no haya errores cuando StudentId y CourseId son valores válidos (mayores a 0).
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_StudentId_And_CourseId_Are_Valid()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 1, CourseId = 1 };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.StudentId);
        result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
    }
}