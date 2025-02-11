using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetByStudentAndCourse;

public class GetEnrollmentByStudentAndCourseQueryHandler :
    IRequestHandler<GetEnrollmentByStudentAndCourseQuery, ErrorOr<EnrollmentDto>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;

    public GetEnrollmentByStudentAndCourseQueryHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<EnrollmentDto>> Handle(GetEnrollmentByStudentAndCourseQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetByStudentAndCourseAsync(request.StudentId, request.CourseId);
        if (enrollment == null) 
        {
            return Errors.Enrollment.IdStudentAndIsCourseNotFound;
        }

        return _mapper.Map<EnrollmentDto>(enrollment);
    }
}