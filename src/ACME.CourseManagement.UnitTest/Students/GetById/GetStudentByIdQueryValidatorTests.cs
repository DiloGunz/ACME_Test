using ACME.CourseManagement.Service.Application.Students.GetById;
using FluentValidation.TestHelper;

namespace ACME.CourseManagement.UnitTest.Students.GetById;

public class GetStudentByIdQueryValidatorTests
{
    private readonly GetStudentByIdQueryValidator _validator;

    public GetStudentByIdQueryValidatorTests()
    {
        _validator = new GetStudentByIdQueryValidator();
    }

    /// <summary>
    /// Verifica que el validador genere un error si el Id es 0
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Id_Is_Zero()
    {
        // Arrange
        var query = new GetStudentByIdQuery { Id = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    /// Verifica que el validador genere un error si el Id es negativo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Id_Is_Negative()
    {
        var query = new GetStudentByIdQuery { Id = -1 };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }


    /// <summary>
    /// Verifica que no haya errores si el Id es mayor que 0.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Greater_Than_Zero()
    {
        var query = new GetStudentByIdQuery { Id = 5 };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}