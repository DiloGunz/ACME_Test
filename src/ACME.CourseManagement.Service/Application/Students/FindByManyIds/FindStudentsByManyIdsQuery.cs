using ACME.CourseManagement.Service.Application.Students.Common;

namespace ACME.CourseManagement.Service.Application.Students.FindByManyIds;

public record FindStudentsByManyIdsQuery : IRequest<ErrorOr<IReadOnlyList<StudentDto>>>
{
	public FindStudentsByManyIdsQuery()
	{

	}

	public FindStudentsByManyIdsQuery(List<long> studentsIds)
    {
		StudentsIds = studentsIds;
	}
    public List<long> StudentsIds { get; set; }
}
