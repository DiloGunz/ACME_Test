using ACME.CourseManagement.Service.Domain.DomainEnums;

namespace ACME.CourseManagement.Service.Application.Payments.Common;

public record PaymentDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public decimal AmountPaid { get; set; }
    public Enums.PaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
}