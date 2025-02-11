namespace ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;

public class GetPaymentByStudentAndCourseQueryValidator : AbstractValidator<GetPaymentByStudentAndCourseQuery>
{
    public GetPaymentByStudentAndCourseQueryValidator()
    {
        RuleFor(x => x.StudentId).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}