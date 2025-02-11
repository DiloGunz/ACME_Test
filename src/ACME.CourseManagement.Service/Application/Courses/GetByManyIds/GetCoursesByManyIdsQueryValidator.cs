namespace ACME.CourseManagement.Service.Application.Courses.GetByManyIds;

public class GetCoursesByManyIdsQueryValidator : AbstractValidator<GetCoursesByManyIdsQuery>
{
	public GetCoursesByManyIdsQueryValidator()
	{
		RuleFor(x => x.CourseIds).Must(x => x.Any());
	}
}