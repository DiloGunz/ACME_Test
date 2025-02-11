using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByManyCourses;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Courses.FindByDateRange;

public class FindCoursesByDateRangeQueryHandler : IRequestHandler<FindCoursesByDateRangeQuery, ErrorOr<IReadOnlyList<CourseDetailsDto>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FindCoursesByDateRangeQueryHandler(ICourseRepository courseRepository, IMediator mediator, IMapper mapper)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<CourseDetailsDto>>> Handle(FindCoursesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.FindByDateRangeAsync(request.DateFrom, request.DateTo);

        if (!courses.Any())
        {
            return Errors.Course.NotRecordsFoundInDateRange;
        }

        var courseIds = courses.Select(c => c.Id).ToList();
        var enrollments = await _mediator.Send(new GetEnrolledByManyCoursesQuery(courseIds), cancellationToken);

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
