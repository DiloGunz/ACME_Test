using ACME.CourseManagement.Service.Application.Enrollments.Common;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;

public record GetEnrollmentByEnrollmentDateQuery : IRequest<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}