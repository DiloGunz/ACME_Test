using ACME.CourseManagement.Service.Application.Enrollments.Common;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByCourse;

public record GetEnrolledByCourseQuery : IRequest<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
    public GetEnrolledByCourseQuery()
    {

    }

    public GetEnrolledByCourseQuery(long courseId)
    {
        CourseId = courseId;
    }

    public long CourseId { get; set; }
}
