using ACME.CourseManagement.Service.Application.Payments.Common;

namespace ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;

public record GetPaymentByStudentAndCourseQuery : IRequest<ErrorOr<PaymentDto>>
{
    public long CourseId { get; set; }
    public long StudentId { get; set; }
}
