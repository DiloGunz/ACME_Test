using ACME.CourseManagement.Service.Application.Students.Create;
using FluentValidation.TestHelper;

namespace ACME.CourseManagement.UnitTest.Students.Create;

public class CreateStudentValidatorTests
{
    private readonly CreateStudentCommandValidator _validator;

    public CreateStudentValidatorTests()
    {
        _validator = new CreateStudentCommandValidator();
    }

    [Fact]
    public void Should_Return_Error_When_FirstName_Is_Empty()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "",
            LastName = "Perez",
            Age = 20,
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Return_Error_When_LastName_Is_Empty()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "",
            Age = 20,
            DocumentNumber = "123"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Return_Error_When_DocumentNumber_Is_Empty()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = ""
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }
}