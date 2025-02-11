namespace ACME.CourseManagement.Service.Application.Students.GetById;

public class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
{
	public GetStudentByIdQueryValidator()
	{
		RuleFor(x=>x.Id).GreaterThan(0);
	}
}