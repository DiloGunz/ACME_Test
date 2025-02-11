namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByCourse;

public class GetEnrolledByCourseQueryValidator : AbstractValidator<GetEnrolledByCourseQuery>
{
	public GetEnrolledByCourseQueryValidator()
	{
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}