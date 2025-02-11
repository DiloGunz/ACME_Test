using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;

public class GetStudentByDocumentNumberQueryHandler :
    IRequestHandler<GetStudentByDocumentNumberQuery, ErrorOr<StudentDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentByDocumentNumberQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<StudentDto>> Handle(GetStudentByDocumentNumberQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByDocumentNumberAsync(request.DocumentNumber);
        if (student == null)
        {
            return Errors.Student.NotExistDocumentNumber;
        }

        return _mapper.Map<StudentDto>(student);
    }
}