using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Courses.GetByManyIds;

public class GetCoursesByManyIdsQueryHandler :
    IRequestHandler<GetCoursesByManyIdsQuery, ErrorOr<IReadOnlyList<CourseDto>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCoursesByManyIdsQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<CourseDto>>> Handle(GetCoursesByManyIdsQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetByManyidsAsync(request.CourseIds);
        if (!courses.Any())
        {
            return Errors.Course.IdNotFound;
        }

        return _mapper.Map<List<CourseDto>>(courses);
    }
}