using ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Application.Students.Create;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ErrorOr<long>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentRepository _studentRepository;
    private readonly IMediator _mediator;

    public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IStudentRepository studentRepository, IMediator mediator)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ErrorOr<long>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        if (request.Age < 18)
        {
            return Errors.Student.AgeNotValid;
        }

        var existDocumentNumberQuery = new GetStudentByDocumentNumberQuery()
        {
            DocumentNumber = request.DocumentNumber,
        };

        var resultExistStudent = await _mediator.Send(existDocumentNumberQuery);

        if (!resultExistStudent.IsError)
        {
            return Errors.Student.AlreadyExistDocumentNumber;
        }

        var student = new Student(request.FirstName, request.LastName, request.Age, request.DocumentNumber);
        await _studentRepository.AddAsync(student);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return student.Id;
    }
}