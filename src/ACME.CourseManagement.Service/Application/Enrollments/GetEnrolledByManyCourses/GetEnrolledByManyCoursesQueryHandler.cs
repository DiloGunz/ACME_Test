using ACME.CourseManagement.Service.Application.Courses.GetByManyIds;
using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByManyCourses;

public class GetEnrolledByManyCoursesQueryHandler :
    IRequestHandler<GetEnrolledByManyCoursesQuery, ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetEnrolledByManyCoursesQueryHandler(IEnrollmentRepository enrollmentRepository, IMediator mediator, IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>> Handle(GetEnrolledByManyCoursesQuery request, CancellationToken cancellationToken)
    {
        var course = await _mediator.Send(new GetCoursesByManyIdsQuery(request.CourseIds));
        if (course.IsError)
        {
            return Domain.DomainErrors.Errors.Course.IdNotFound;
        }

        var enrollments = await _enrollmentRepository.GetAllByManyCoursesAsync(request.CourseIds);

        if (!enrollments.Any())
        {
            return new List<EnrollmentDetailsDto>();
        }

        var studentsIds = enrollments.Select(x => x.StudentId).ToList();

        var students = await _mediator.Send(new FindStudentsByManyIdsQuery(enrollments.Select(x => x.StudentId).Distinct().ToList()), cancellationToken);

        if (students.IsError)
        {
            return Domain.DomainErrors.Errors.Student.NotRecordsFoundByManyIds;
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