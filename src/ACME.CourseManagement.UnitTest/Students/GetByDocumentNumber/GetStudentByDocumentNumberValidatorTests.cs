using ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;
using FluentValidation.TestHelper;

namespace ACME.CourseManagement.UnitTest.Students.GetByDocumentNumber;

public class GetStudentByDocumentNumberValidatorTests
{
    private readonly GetStudentByDocumentNumberQueryValidator _validator;

    public GetStudentByDocumentNumberValidatorTests()
    {
        _validator = new GetStudentByDocumentNumberQueryValidator();
    }

    [Fact]
    public void Should_Return_Error_When_DocumentNumber_Is_Empty()
    {
        var command = new GetStudentByDocumentNumberQuery()
        {
            DocumentNumber = ""
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }

    [Fact]
    public void Should_Return_Error_When_DocumentNumber_Is_Null()
    {
        var command = new GetStudentByDocumentNumberQuery()
        {
            DocumentNumber = null!
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }

    [Fact]
    public void Should_Return_Error_When_DocumentNumber_Is_Spaces()
    {
        var command = new GetStudentByDocumentNumberQuery()
        {
            DocumentNumber = "            "
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }
}