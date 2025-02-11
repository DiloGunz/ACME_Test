using ACME.CourseManagement.Service.Application.Payments.Create;
using ACME.CourseManagement.Service.Domain.DomainEnums;

namespace ACME.CourseManagement.UnitTest.Payments.Create;

public class CreatePaymentValidatorTests
{
    private readonly CreatePaymentCommandValidator _validator;

    public CreatePaymentValidatorTests()
    {
        _validator = new CreatePaymentCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_AmountPaid_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new CreatePaymentCommand { AmountPaid = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AmountPaid);
    }

    [Fact]
    public void Should_Not_Have_Error_When_AmountPaid_Is_Greater_Than_Zero()
    {
        var model = new CreatePaymentCommand { AmountPaid = 10 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AmountPaid);
    }

    [Fact]
    public void Should_Have_Error_When_CourseId_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new CreatePaymentCommand { CourseId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CourseId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_CourseId_Is_Greater_Than_Zero()
    {
        var model = new CreatePaymentCommand { CourseId = 5 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
    }

    [Fact]
    public void Should_Have_Error_When_StudentId_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new CreatePaymentCommand { StudentId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StudentId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_StudentId_Is_Greater_Than_Zero()
    {
        var model = new CreatePaymentCommand { StudentId = 3 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.StudentId);
    }

    [Theory]
    [InlineData(Enums.PaymentStatus.Pending)]
    [InlineData(Enums.PaymentStatus.Completed)]
    [InlineData(Enums.PaymentStatus.Failed)]
    [InlineData(Enums.PaymentStatus.Canceled)]
    [InlineData(Enums.PaymentStatus.Refunded)]
    [InlineData(Enums.PaymentStatus.Processing)]
    public void Should_Not_Have_Error_When_PaymentStatus_Is_Valid(Enums.PaymentStatus status)
    {
        var model = new CreatePaymentCommand { PaymentStatus = status };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.PaymentStatus);
    }

    [Fact]
    public void Should_Have_Error_When_PaymentStatus_Is_Not_In_Enum()
    {
        var model = new CreatePaymentCommand { PaymentStatus = (Enums.PaymentStatus)999 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PaymentStatus);
    }

    [Fact]
    public void Should_Have_Error_When_PaymentDate_Is_In_The_Future()
    {
        var model = new CreatePaymentCommand { PaymentDate = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PaymentDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_PaymentDate_Is_In_The_Past()
    {
        var model = new CreatePaymentCommand { PaymentDate = DateTime.UtcNow.AddDays(-1) };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.PaymentDate);
    }
}