using ACME.CourseManagement.Service.Application.Courses.Create;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using ACME.CourseManagement.Service.Domain.Primitives;
using ACME.CourseManagement.Service.Infraestructure.Persistence;
using ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;
using Moq;

namespace ACME.CourseManagement.UnitTest.Courses.Create;

public class CreateCourseCommandHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateCourseCommandHandler _handler;

    public CreateCourseCommandHandlerTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateCourseCommandHandler(_courseRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    /// <summary>
    /// Valida que se retorne el ID del curso cuando la creación es exitosa usando fake repository
    /// </summary>
    [Fact]
    public async Task Handle_Should_Return_CourseId_When_Course_Is_Created_Successfully_Fake()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 500,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        var course = new Course(command.Name, command.EnrollmentFee, command.Capacity, command.StartDate, command.EndDate);

        ICourseRepository repository = new CourseRepository();
        IUnitOfWork unitOfWork = new UnitOfWork();

        var handler = new CreateCourseCommandHandler(repository, unitOfWork);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Valida que se retorne el ID del curso cuando la creación es exitosa.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Return_CourseId_When_Course_Is_Created_Successfully()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 500,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        var course = new Course(command.Name, command.EnrollmentFee, command.Capacity, command.StartDate, command.EndDate);

        _courseRepositoryMock.Setup(repo => repo.ExistsNameAsync(command.Name))
            .ReturnsAsync(false); // Simula que el curso no existe previamente

        _courseRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Course>()))
            .Callback<Course>(c => c.Id = 100) // Simula la asignación de un ID
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(100);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el nombre del curso ya existe en el sistema.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Course_Name_Already_Exists()
    {
        var command = new CreateCourseCommand
        {
            Capacity = 30,
            EnrollmentFee = 500,
            Name = "Matematicas",
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        _courseRepositoryMock.Setup(repo => repo.ExistsNameAsync(command.Name))
            .ReturnsAsync(true); // Simula que el curso ya existe

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.NameAlreadyExists);
    }

    
}