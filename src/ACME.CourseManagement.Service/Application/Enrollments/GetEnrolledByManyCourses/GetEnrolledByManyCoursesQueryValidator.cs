namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByManyCourses;

public class GetEnrolledByManyCoursesQueryValidator : AbstractValidator<GetEnrolledByManyCoursesQuery>
{
	public GetEnrolledByManyCoursesQueryValidator()
	{
		RuleFor(x => x.CourseIds).Must(x => x.Any());
	}
}