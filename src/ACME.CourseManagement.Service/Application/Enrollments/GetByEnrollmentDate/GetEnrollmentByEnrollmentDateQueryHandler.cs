using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;

public class GetEnrollmentByEnrollmentDateQueryHandler :
    IRequestHandler<GetEnrollmentByEnrollmentDateQuery, ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetEnrollmentByEnrollmentDateQueryHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper, IMediator mediator)
    {
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ErrorOr<IReadOnlyList<EnrollmentDetailsDto>>> Handle(GetEnrollmentByEnrollmentDateQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _enrollmentRepository.GetByEnrollmentDateAsync(request.DateFrom, request.DateTo);

        if (!enrollments.Any())
        {
            return Errors.Enrollment.EnrollmentDateNotFound;
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
