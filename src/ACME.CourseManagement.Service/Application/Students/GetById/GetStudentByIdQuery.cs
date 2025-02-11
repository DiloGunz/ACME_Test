using ACME.CourseManagement.Service.Application.Students.Common;

namespace ACME.CourseManagement.Service.Application.Students.GetById;

public record GetStudentByIdQuery : IRequest<ErrorOr<StudentDto>>
{
	public GetStudentByIdQuery()
	{

	}
	public GetStudentByIdQuery(long id)
	{
		Id = id;
	}
    public long Id { get; set; }
}