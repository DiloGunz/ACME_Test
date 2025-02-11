using ACME.CourseManagement.Service.Application.Courses.Common;

namespace ACME.CourseManagement.Service.Application.Courses.FindByEnrollmentDate;

public record FindCoursesByEnrollmentDateQuery : IRequest<ErrorOr<IReadOnlyList<CourseDetailsDto>>>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}