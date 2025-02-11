using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using FluentAssertions;
using Moq;

namespace ACME.CourseManagement.UnitTest.Enrollments.GetNumberEnrolledByCourse;

public class GetNumberStudentEnrolledByCourseQueryHandlerTests
{
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetNumberStudentEnrolledByCourseQueryHandler _handler;

    public GetNumberStudentEnrolledByCourseQueryHandlerTests()
    {
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new GetNumberStudentEnrolledByCourseQueryHandler(_enrollmentRepositoryMock.Object, _mediatorMock.Object);
    }

    /// <summary>
    /// Valida que se retorne un error cuando el curso no existe.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Course_NotFound()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = 1 };

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == query.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Course.IdNotFound);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.IdNotFound);
    }

    /// <summary>
    /// Valida que se retorne el número correcto de estudiantes inscritos cuando el curso existe.
    /// </summary>
    [Fact]
    public async Task Should_Return_Number_Of_Students_When_Course_Exists()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = 1 };
        var course = new CourseDto 
        { 
            Id = 1, 
            Name = "Matematicas", 
            Capacity = 100, 
            Enable = true, 
            EnrollmentFee = 100, 
            StartDate = DateTime.UtcNow.AddMonths(1), 
            EndDate = DateTime.UtcNow.AddMonths(2) 
        };

        var enrollments = new List<Enrollment>
        {
            new Enrollment { Id = 1, StudentId = 1, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-2) },
            new Enrollment { Id = 2, StudentId = 2, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-2) }
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == query.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _enrollmentRepositoryMock.Setup(repo => repo.GetAllByCourseAsync(query.CourseId))
            .ReturnsAsync(enrollments);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(enrollments.Count);
    }

    /// <summary>
    /// Valida que se retorne 0 cuando no hay estudiantes inscritos en el curso.
    /// </summary>
    [Fact]
    public async Task Should_Return_Zero_When_No_Students_Enrolled()
    {
        var query = new GetNumberStudentEnrolledByCourseQuery { CourseId = 1 };
        var course = new CourseDto
        {
            Id = 1,
            Name = "Matematicas",
            Capacity = 100,
            Enable = true,
            EnrollmentFee = 100,
            StartDate = DateTime.UtcNow.AddMonths(1),
            EndDate = DateTime.UtcNow.AddMonths(2)
        };
        var enrollments = new List<Enrollment>(); // Lista vacía

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == query.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _enrollmentRepositoryMock.Setup(repo => repo.GetAllByCourseAsync(query.CourseId))
            .ReturnsAsync(enrollments);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(0);
    }
}