using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Enrollments.Create;
using ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.GetById;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using ACME.CourseManagement.Service.Domain.Primitives;
using Moq;

namespace ACME.CourseManagement.UnitTest.Enrollments.Create;

public class CreateEnrollmentCommandHandlerTests
{
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CreateEnrollmentCommandHandler _handler;

    public CreateEnrollmentCommandHandlerTests()
    {
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new CreateEnrollmentCommandHandler(_unitOfWorkMock.Object, _enrollmentRepositoryMock.Object, _mediatorMock.Object);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el estudiante no existe.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Student_NotFound()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,   
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Student.IdNotFound);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Student.IdNotFound);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el curso no existe.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Course_NotFound()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,
        };
        var student = new StudentDto
        { 
            Id = 1, 
            FirstName = "Juan", 
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Course.IdNotFound);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.IdNotFound);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el curso está deshabilitado.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Course_Is_Disabled()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,
        };
        var student = new StudentDto
        {
            Id = 1,
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };
        var course = new CourseDto 
        { 
            Id = 1, 
            Name = "Matematicas", 
            Enable = false ,
            Capacity = 100,
            EndDate = DateTime.UtcNow.AddMonths(2),
            StartDate = DateTime.UtcNow.AddMonths(1)
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.Disable);
    }

    /// <summary>
    /// Valida que se retorne un error cuando no hay vacantes disponibles en el curso.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_No_Vacancies_Available()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,
        };
        var student = new StudentDto
        {
            Id = 1,
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };
        var course = new CourseDto
        {
            Id = 1,
            Name = "Matematicas",
            Enable = true,
            Capacity = 2,
            EndDate = DateTime.UtcNow.AddMonths(2),
            StartDate = DateTime.UtcNow.AddMonths(1)
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _mediatorMock.Setup(m => m.Send(It.Is<GetNumberStudentEnrolledByCourseQuery>(q => q.CourseId == course.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course.Capacity); // No vacantes disponibles

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.InsufficientVacancies);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el estudiante ya está matriculado en el curso.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Student_Is_Already_Enrolled()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,
        };
        var student = new StudentDto
        {
            Id = 1,
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };
        var course = new CourseDto
        {
            Id = 1,
            Name = "Matematicas",
            Enable = true,
            Capacity = 10,
            EndDate = DateTime.UtcNow.AddMonths(2),
            StartDate = DateTime.UtcNow.AddMonths(1)
        };
        var enrollment = new Enrollment 
        { 
            Id = 1, 
            StudentId = student.Id, 
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _enrollmentRepositoryMock.Setup(repo => repo.GetByStudentAndCourseAsync(student.Id, course.Id))
            .ReturnsAsync(enrollment);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Enrollment.StudentHasAlreadyBeenEnrolled);
    }

    /// <summary>
    /// Valida que se retorne el ID de la matrícula cuando el proceso es exitoso.
    /// </summary>
    [Fact]
    public async Task Should_Return_EnrollmentId_When_Enrollment_Is_Successful()
    {
        var command = new CreateEnrollmentCommand
        {
            CourseId = 1,
            StudentId = 1,
            EnrollmentDate = DateTime.UtcNow,
        };
        var student = new StudentDto
        {
            Id = 1,
            FirstName = "Juan",
            LastName = "Perez",
            Age = 20,
            DocumentNumber = "123"
        };
        var course = new CourseDto
        {
            Id = 1,
            Name = "Matematicas",
            Enable = true,
            Capacity = 10,
            EndDate = DateTime.UtcNow.AddMonths(2),
            StartDate = DateTime.UtcNow.AddMonths(1)
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _enrollmentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Enrollment>()))
            .Callback<Enrollment>(e => e.Id = 100)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(100);
    }
}