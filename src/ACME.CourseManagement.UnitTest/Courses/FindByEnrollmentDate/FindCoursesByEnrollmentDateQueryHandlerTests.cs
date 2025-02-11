using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Courses.FindByEnrollmentDate;
using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Application.Enrollments.GetByEnrollmentDate;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;
using Moq;

namespace ACME.CourseManagement.UnitTest.Courses.FindByEnrollmentDate;

/// <summary>
/// Test Unitarios para mostrar cursos con studiantes matriculados por fecha de inscripcion
/// </summary>
public class FindCoursesByEnrollmentDateQueryHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindCoursesByEnrollmentDateQueryHandler _handler;

    public FindCoursesByEnrollmentDateQueryHandlerTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindCoursesByEnrollmentDateQueryHandler(_courseRepositoryMock.Object, _mediatorMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Prueba que verifica si el manejador devuelve un error cuando el mediador retorna un error.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_Should_Return_Error_When_Mediator_Returns_Error()
    {
        var request = new FindCoursesByEnrollmentDateQuery { DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEnrollmentByEnrollmentDateQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Errors.Enrollment.EnrollmentDateNotFound);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Enrollment.EnrollmentDateNotFound);
    }

    /// <summary>
    /// Prueba que verifica que el manejador devuelve una lista vacía cuando no se encuentran inscripciones.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Enrollments_Found()
    {
        var request = new FindCoursesByEnrollmentDateQuery { DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEnrollmentByEnrollmentDateQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new List<EnrollmentDetailsDto>());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    /// <summary>
    /// Prueba que verifica si el manejador devuelve un error cuando no se encuentran cursos asociados a las inscripciones
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_Should_Return_Error_When_No_Courses_Found()
    {
        var request = new FindCoursesByEnrollmentDateQuery { DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow };
        var enrollments = new List<EnrollmentDetailsDto>
        {
            new EnrollmentDetailsDto { CourseId = 1, Student = new StudentDto() }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEnrollmentByEnrollmentDateQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(enrollments);

        _courseRepositoryMock.Setup(c => c.GetByManyidsAsync(It.IsAny<List<long>>()))
                             .ReturnsAsync(new List<Course>());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.NotRecordsFoundInDateRange);
    }

    /// <summary>
    /// Prueba que verifica si el manejador retorna los cursos mapeados con las inscripciones correspondientes.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_Should_Return_Mapped_Courses_With_Enrollments()
    {
        var request = new FindCoursesByEnrollmentDateQuery { DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow };
        var courseId = 1;
        var enrollments = new List<EnrollmentDetailsDto>
        {
            new EnrollmentDetailsDto { CourseId = courseId, Student = new StudentDto { FirstName = "Juan", LastName = "Perez" } }
        };
        var courses = new List<Course> { new Course { Id = courseId, Name = "Matematicas" } };
        var mappedCourse = new CourseDetailsDto { Id = courseId, Name = "Matematicas" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEnrollmentByEnrollmentDateQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(enrollments);
        _courseRepositoryMock.Setup(c => c.GetByManyidsAsync(It.IsAny<List<long>>()))
                             .ReturnsAsync(courses);
        _mapperMock.Setup(m => m.Map<CourseDetailsDto>(It.IsAny<Course>()))
                   .Returns(mappedCourse);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().HaveCount(1);
        result.Value.First().EnrolledStudents.Should().HaveCount(1);
        result.Value.First().EnrolledStudents.First().FirstName.Should().Be("Juan");
        result.Value.First().EnrolledStudents.First().LastName.Should().Be("Perez");
    }
}