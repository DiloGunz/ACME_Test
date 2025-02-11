namespace ACME.CourseManagement.Service.Application.Payments.Create;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.AmountPaid).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.StudentId).GreaterThan(0);
        RuleFor(x => x.PaymentStatus).IsInEnum();
        RuleFor(x => x.PaymentDate).Must(x => x < DateTime.UtcNow);
    }
}