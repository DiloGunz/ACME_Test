using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using AutoMapper;
using Moq;

namespace ACME.CourseManagement.UnitTest.Enrollments.GetByEnrollmentDate;

public class GetEnrollmentByEnrollmentDateQueryHandlerTests
{
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetEnrollmentByEnrollmentDateQueryHandler _handler;

    public GetEnrollmentByEnrollmentDateQueryHandlerTests()
    {
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();

        _handler = new GetEnrollmentByEnrollmentDateQueryHandler(
            _enrollmentRepositoryMock.Object,
            _mapperMock.Object,
            _mediatorMock.Object
        );
    }

    /// <summary>
    /// Verifica que el handler retorne un error cuando no hay matrículas en el rango de fechas.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Return_Error_When_No_Enrollments_Found()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-10),
            DateTo = DateTime.UtcNow
        };

        _enrollmentRepositoryMock
            .Setup(repo => repo.GetByEnrollmentDateAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(new List<Enrollment>());

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Enrollment.EnrollmentDateNotFound);
    }

    /// <summary>
    /// Verifica que el handler retorne un error cuando la consulta de estudiantes falla.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Return_Error_When_Students_Not_Found()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-10),
            DateTo = DateTime.UtcNow
        };

        var enrollments = new List<Enrollment>
        {
            new Enrollment { StudentId = 1 }
        };

        _enrollmentRepositoryMock
            .Setup(repo => repo.GetByEnrollmentDateAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(enrollments);

        _mediatorMock
            .Setup(med => med.Send(It.IsAny<FindStudentsByManyIdsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Student.NotRecordsFoundByManyIds);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Student.NotRecordsFoundByManyIds);
    }

    /// <summary>
    /// Verifica que el handler retorne una lista de matrículas con información del estudiante.
    /// </summary>
    [Fact]
    public async Task Handle_Should_Return_Enrollments_With_Student_Data()
    {
        var query = new GetEnrollmentByEnrollmentDateQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-10),
            DateTo = DateTime.UtcNow
        };

        var studentId = 1;
        var enrollments = new List<Enrollment>
        {
            new Enrollment { StudentId = studentId }
        };

        var students = new List<StudentDto>
        {
            new StudentDto { Id = studentId, FirstName = "Juan", LastName = "Perez" }
        };

        _enrollmentRepositoryMock
            .Setup(repo => repo.GetByEnrollmentDateAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(enrollments);

        _mediatorMock
            .Setup(med => med.Send(It.IsAny<FindStudentsByManyIdsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        _mapperMock
            .Setup(mapper => mapper.Map<EnrollmentDetailsDto>(It.IsAny<Enrollment>()))
            .Returns(new EnrollmentDetailsDto());

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().HaveCount(1);
    }
}
