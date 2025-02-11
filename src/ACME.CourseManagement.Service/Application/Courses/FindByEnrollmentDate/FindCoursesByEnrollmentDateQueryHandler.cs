using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Courses.FindByEnrollmentDate;

public class FindCoursesByEnrollmentDateQueryHandler :
    IRequestHandler<FindCoursesByEnrollmentDateQuery, ErrorOr<IReadOnlyList<CourseDetailsDto>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FindCoursesByEnrollmentDateQueryHandler(ICourseRepository courseRepository, IMediator mediator, IMapper mapper)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<CourseDetailsDto>>> Handle(FindCoursesByEnrollmentDateQuery request, CancellationToken cancellationToken)
    {
        var enrollmentsQuery = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = request.DateFrom,
            DateTo = request.DateTo
        };

        var enrollments = await _mediator.Send(enrollmentsQuery);

        if (enrollments.IsError)
        {
            return enrollments.FirstError;
        }

        if (!enrollments.Value.Any())
        {
            return new List<CourseDetailsDto>();
        }

        var coursesIds = enrollments.Value.Select(x => x.CourseId).ToList();
        var courses = await _courseRepository.GetByManyidsAsync(coursesIds);

        if (!courses.Any())
        {
            return Errors.Course.NotRecordsFoundInDateRange;
        }

        var enrollmentDictionary = enrollments.Value.GroupBy(e => e.CourseId)
                                               .ToDictionary(g => g.Key, g => g.Select(e => e.Student).ToList());

        var result = courses.Select(course =>
        {
            var dto = _mapper.Map<CourseDetailsDto>(course);
            dto.EnrolledStudents = enrollmentDictionary.GetValueOrDefault(course.Id, new List<StudentDto>());
            return dto;
        }).ToList();

        return result;
    }
}