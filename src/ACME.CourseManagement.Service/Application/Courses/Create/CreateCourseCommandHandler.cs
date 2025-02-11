
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Application.Courses.Create;
/// <summary>
/// Manejador del comando CreateCourseCommand.
/// Se encarga de procesar la creación de un curso.
/// </summary>
public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, ErrorOr<long>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor que inicializa las dependencias del manejador.
    /// </summary>
    /// <param name="courseRepository">Repositorio de cursos.</param>
    /// <param name="unitOfWork">Unidad de trabajo para manejar transacciones.</param>
    public CreateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Maneja la creación de un curso a partir del comando recibido.
    /// </summary>
    /// <param name="request">Comando con la información del curso a crear.</param>
    /// <param name="cancellationToken">Token de cancelación de la tarea.</param>
    /// <returns>El ID del curso creado o un error.</returns>
    public async Task<ErrorOr<long>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var existCourse = await _courseRepository.ExistsNameAsync(request.Name);
        if (existCourse)
        {
            return Errors.Course.NameAlreadyExists;
        }
        var course = new Course(request.Name, request.EnrollmentFee, request.Capacity, request.StartDate, request.EndDate);
        await _courseRepository.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return course.Id;
    }
}
