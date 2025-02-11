using ACME.CourseManagement.Service.Domain.DomainEnums;
using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Payments;

public class Payment
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public decimal AmountPaid { get; set; }
    public Enums.PaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }

    public Payment()
    {

    }
    public Payment(long studentId, long courseId, decimal amountPaid, Enums.PaymentStatus paymentStatus, DateTime paymentDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        StudentId = studentId;
        CourseId = courseId;
        AmountPaid = amountPaid;
        PaymentStatus = paymentStatus;
        PaymentDate = paymentDate;
    }
}