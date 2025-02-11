using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;

public class GetNumberStudentEnrolledByCourseQueryHandler :
    IRequestHandler<GetNumberStudentEnrolledByCourseQuery, ErrorOr<int>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMediator _mediator;

    public GetNumberStudentEnrolledByCourseQueryHandler(IEnrollmentRepository enrollmentRepository, IMediator mediator)
    {
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ErrorOr<int>> Handle(GetNumberStudentEnrolledByCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(request.CourseId));
        if (course.IsError)
        {
            return Errors.Course.IdNotFound;
        }

        var enrollments = await _enrollmentRepository.GetAllByCourseAsync(request.CourseId);

        return enrollments.Count();
    }
}
