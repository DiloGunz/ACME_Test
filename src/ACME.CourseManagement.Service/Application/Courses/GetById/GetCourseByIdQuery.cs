using ACME.CourseManagement.Service.Application.Courses.Common;

namespace ACME.CourseManagement.Service.Application.Courses.GetById;

/// <summary>
/// Representa una consulta para obtener un curso por su identificador.
/// Implementa IRequest con un resultado de tipo ErrorOr<CourseDto>.
/// </summary>
/// <param name="Id">Identificador único del curso a buscar.</param>
public record GetCourseByIdQuery : IRequest<ErrorOr<CourseDto>>
{
	public GetCourseByIdQuery()
	{

	}
	public GetCourseByIdQuery(long courseId)
	{
		CourseId = courseId;
	}
    public long CourseId { get; set; }
}
