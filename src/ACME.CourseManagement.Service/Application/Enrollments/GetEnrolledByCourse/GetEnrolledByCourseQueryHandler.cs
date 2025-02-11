using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using AutoMapper;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByCourse;

public class GetEnrolledByCourseQueryHandler :
    IRequestHandler<GetEnrolledByCourseQuery, ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetEnrolledByCourseQueryHandler(IEnrollmentRepository enrollmentRepository, IMediator mediator, IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>> Handle(GetEnrolledByCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(request.CourseId));
        if (course.IsError)
        {
            return Errors.Course.IdNotFound;
        }

        var enrollments = await _enrollmentRepository.GetAllByCourseAsync(request.CourseId);
        if (!enrollments.Any()) 
        {
            return new List<EnrollmentDetailsDto>();
        }

        var studentsIds = enrollments.Select(x => x.StudentId).ToList();

        var students = await _mediator.Send(new FindStudentsByManyIdsQuery(enrollments.Select(x => x.StudentId).Distinct().ToList()), cancellationToken);

        if (students.IsError)
        {
            return Errors.Student.NotRecordsFoundByManyIds;
        }

        var studentDictionary = students.Value.ToDictionary(s => s.Id);

        var result = enrollments.Select(e =>
        {
            var dto = _mapper.Map<EnrollmentDetailsDto>(e);
            if (studentDictionary.TryGetValue(e.StudentId, out var student))
            {
                dto.Student = student;
            }
            return dto;
        }).ToList();

        return result;
    }
}
