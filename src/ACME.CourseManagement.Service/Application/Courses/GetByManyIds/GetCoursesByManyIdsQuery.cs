using ACME.CourseManagement.Service.Application.Courses.Common;

namespace ACME.CourseManagement.Service.Application.Courses.GetByManyIds;

public record GetCoursesByManyIdsQuery : IRequest<ErrorOr<IReadOnlyList<CourseDto>>>
{
	public GetCoursesByManyIdsQuery()
	{

	}

	public GetCoursesByManyIdsQuery(List<long> courseIds)
	{
		CourseIds = courseIds;
	}
    public List<long> CourseIds { get; set; }
}