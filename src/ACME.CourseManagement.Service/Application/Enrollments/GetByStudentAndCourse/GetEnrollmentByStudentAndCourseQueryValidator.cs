namespace ACME.CourseManagement.Service.Application.Enrollments.GetByStudentAndCourse;

public class GetEnrollmentByStudentAndCourseQueryValidator : AbstractValidator<GetEnrollmentByStudentAndCourseQuery>
{
	public GetEnrollmentByStudentAndCourseQueryValidator()
	{
		RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.StudentId).GreaterThan(0);
    }
}