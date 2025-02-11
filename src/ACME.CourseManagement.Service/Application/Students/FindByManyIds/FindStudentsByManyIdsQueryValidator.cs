namespace ACME.CourseManagement.Service.Application.Students.FindByManyIds;

public class FindStudentsByManyIdsQueryValidator : AbstractValidator<FindStudentsByManyIdsQuery>
{
	public FindStudentsByManyIdsQueryValidator()
	{
		RuleFor(x => x.StudentsIds).Must(x => x.Any());
	}
}
