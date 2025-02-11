using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Students.GetById;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, ErrorOr<StudentDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentByIdQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetAsync(request.Id);

        if (student is null)
        {
            return Errors.Student.IdNotFound;
        }

        return _mapper.Map<StudentDto>(student);
    }
}