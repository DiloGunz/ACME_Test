using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Courses.GetById;


/// <summary>
/// Manejador para la consulta GetCourseByIdQuery.
/// Se encarga de recuperar un curso por su identificador.
/// </summary>
public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, ErrorOr<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor que inicializa las dependencias del manejador.
    /// </summary>
    /// <param name="courseRepository">Repositorio de cursos.</param>
    /// <param name="mapper">Mapper para convertir la entidad Course a CourseDto.</param>
    public GetCourseByIdQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Maneja la consulta para obtener un curso por su ID.
    /// </summary>
    /// <param name="request">Consulta con el ID del curso a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación de la tarea.</param>
    /// <returns>Un CourseDto si se encuentra el curso, o un error si no existe.</returns>
    public async Task<ErrorOr<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetAsync(request.CourseId);
        if (course is null)
        {
            return Errors.Course.IdNotFound;
        }
        return _mapper.Map<CourseDto>(course);
    }
}
