namespace ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;

public class GetStudentByDocumentNumberQueryValidator : AbstractValidator<GetStudentByDocumentNumberQuery>
{
	public GetStudentByDocumentNumberQueryValidator()
	{
		RuleFor(x => x.DocumentNumber).NotEmpty();
	}
}