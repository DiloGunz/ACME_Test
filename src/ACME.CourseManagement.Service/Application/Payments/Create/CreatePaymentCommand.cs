using ACME.CourseManagement.Service.Domain.DomainEnums;

namespace ACME.CourseManagement.Service.Application.Payments.Create;

public record CreatePaymentCommand : IRequest<ErrorOr<long>>
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public decimal AmountPaid { get; set; }
    public Enums.PaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
}