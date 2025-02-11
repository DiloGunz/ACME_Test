namespace ACME.CourseManagement.Service.Application.Students.Create;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
	public CreateStudentCommandValidator()
	{
		RuleFor(x=>x.FirstName).NotEmpty();
		RuleFor(x=>x.LastName).NotEmpty();
		RuleFor(x=>x.DocumentNumber).NotEmpty();
	}
}