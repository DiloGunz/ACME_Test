using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Students.FindByManyIds;

public class FindStudentsByManyIdsQueryHandler :
    IRequestHandler<FindStudentsByManyIdsQuery, ErrorOr<IReadOnlyList<StudentDto>>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public FindStudentsByManyIdsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<StudentDto>>> Handle(FindStudentsByManyIdsQuery request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.FindByManyIdsAsync(request.StudentsIds);

        if (!students.Any())
        {
            return Errors.Student.NotRecordsFoundByManyIds;
        }

        return _mapper.Map<List<StudentDto>>(students);
    }
}