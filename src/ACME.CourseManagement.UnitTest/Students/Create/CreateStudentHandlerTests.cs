using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.Create;
using ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using ACME.CourseManagement.Service.Domain.Primitives;
using ACME.CourseManagement.Service.Infraestructure.Persistence;
using ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;

namespace ACME.CourseManagement.UnitTest.Students.Create;

public class CreateStudentHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new CreateStudentCommandHandler(_unitOfWorkMock.Object, _studentRepositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Should_Register_Student_Successfully_Fake()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 18,
            DocumentNumber = "123"
        };

        IStudentRepository repository = new StudentRepository();
        IUnitOfWork unitOfWork = new UnitOfWork();
        var handler = new CreateStudentCommandHandler(unitOfWork, repository, _mediatorMock.Object);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetStudentByDocumentNumberQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Student.NotExistDocumentNumber);

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Register_Student_Successfully()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 18,
            DocumentNumber = "123"
        };

        var student = new Student(command.FirstName, command.LastName, command.Age, command.DocumentNumber);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetStudentByDocumentNumberQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Student.NotExistDocumentNumber);

        _studentRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Student>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeGreaterThan(0);
        _studentRepositoryMock.Invocations.Should().HaveCount(1);
        _unitOfWorkMock.Invocations.Should().HaveCount(1);
    }

    [Fact]
    public async Task Should_Return_Error_When_Student_Is_Underage()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 17,
            DocumentNumber = "123"
        };

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be(Errors.Student.AgeNotValid.Code);
        result.FirstError.Description.Should().Be(Errors.Student.AgeNotValid.Description);
    }


    [Fact]
    public async Task Should_Return_Error_When_Already_Exist_Document_Number()
    {
        var command = new CreateStudentCommand()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };
        var query = new GetStudentByDocumentNumberQuery { DocumentNumber = command.DocumentNumber };

        ErrorOr<StudentDto> studentResponse = new StudentDto()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetStudentByDocumentNumberQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentResponse);


        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be(Errors.Student.AlreadyExistDocumentNumber.Code);
        result.FirstError.Description.Should().Be(Errors.Student.AlreadyExistDocumentNumber.Description);
    }


}