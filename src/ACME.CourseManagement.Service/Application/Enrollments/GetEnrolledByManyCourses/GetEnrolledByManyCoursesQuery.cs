using ACME.CourseManagement.Service.Application.Enrollments.Common;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByManyCourses;

public record GetEnrolledByManyCoursesQuery : IRequest<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
	public GetEnrolledByManyCoursesQuery()
	{

	}

	public GetEnrolledByManyCoursesQuery(List<long> courseIds)
	{
		CourseIds = courseIds;
	}
    public List<long> CourseIds { get; set; }
}