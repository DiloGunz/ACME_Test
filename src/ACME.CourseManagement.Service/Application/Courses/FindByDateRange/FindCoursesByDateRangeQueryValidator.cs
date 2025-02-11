namespace ACME.CourseManagement.Service.Application.Courses.FindByDateRange;

public class FindCoursesByDateRangeQueryValidator : AbstractValidator<FindCoursesByDateRangeQuery>
{
	public FindCoursesByDateRangeQueryValidator()
	{
		RuleFor(x => x.DateFrom).Must(x => x < DateTime.UtcNow && x > DateTime.MinValue);
        RuleFor(x => x.DateTo).Must(x => x < DateTime.UtcNow && x > DateTime.MinValue);

		When(x => x.DateFrom < DateTime.UtcNow && x.DateFrom > DateTime.MinValue, () =>
		{
			RuleFor(x => x).Must(x => x.DateTo > x.DateFrom);
		});
    }
}