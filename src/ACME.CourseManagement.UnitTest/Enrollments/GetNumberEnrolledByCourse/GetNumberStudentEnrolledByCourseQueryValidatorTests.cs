using ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;

namespace ACME.CourseManagement.UnitTest.Enrollments.GetNumberEnrolledByCourse;

public class GetNumberStudentEnrolledByCourseQueryValidatorTests
{
    private readonly GetNumberStudentEnrolledByCourseQueryValidator _validator;

    public GetNumberStudentEnrolledByCourseQueryValidatorTests()
    {
        _validator = new GetNumberStudentEnrolledByCourseQueryValidator();
    }

    /// <summary>
    /// Valida que se genere un error cuando CourseId es igual a 0.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_CourseId_Is_Zero()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    /// <summary>
    /// Valida que se genere un error cuando CourseId es un número negativo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_CourseId_Is_Negative()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = -1 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    /// <summary>
    /// Valida que no haya errores cuando CourseId es un número positivo.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_Course_Id_Is_Valid()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = 5 };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
    }
}