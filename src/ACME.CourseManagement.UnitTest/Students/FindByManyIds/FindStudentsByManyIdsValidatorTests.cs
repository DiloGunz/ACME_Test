using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using FluentValidation.TestHelper;

namespace ACME.CourseManagement.UnitTest.Students.FindByManyIds;

public class FindStudentsByManyIdsValidatorTests
{
    private readonly FindStudentsByManyIdsQueryValidator _validator;

    public FindStudentsByManyIdsValidatorTests()
    {
        _validator = new FindStudentsByManyIdsQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_StudentsIds_Is_Empty()
    {
        var query = new FindStudentsByManyIdsQuery { StudentsIds = new List<long>() };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.StudentsIds);
    }

    [Fact]
    public void Should_Not_Have_Error_When_StudentsIds_Is_Not_Empty()
    {
        var query = new FindStudentsByManyIdsQuery { StudentsIds = new List<long> { 1, 2, 3 } };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.StudentsIds);
    }
}