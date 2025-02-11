namespace ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;

public class GetNumberStudentEnrolledByCourseQueryValidator : AbstractValidator<GetNumberStudentEnrolledByCourseQuery>
{
    public GetNumberStudentEnrolledByCourseQueryValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}