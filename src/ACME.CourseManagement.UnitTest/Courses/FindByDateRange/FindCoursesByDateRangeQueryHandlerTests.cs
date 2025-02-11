using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Courses.FindByDateRange;
using ACME.CourseManagement.Service.Application.Enrollments.Common;
using ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByManyCourses;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;
using Moq;

namespace ACME.CourseManagement.UnitTest.Courses.FindByDateRange;

public class FindCoursesByDateRangeQueryHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindCoursesByDateRangeQueryHandler _handler;

    public FindCoursesByDateRangeQueryHandlerTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindCoursesByDateRangeQueryHandler(_courseRepositoryMock.Object, _mediatorMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Valida que se retorne un error cuando no hay cursos en el rango de fechas especificado.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_No_Courses_Found()
    {
        var query = new FindCoursesByDateRangeQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-30),
            DateTo = DateTime.UtcNow
        };

        _courseRepositoryMock.Setup(repo => repo.FindByDateRangeAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(new List<Course>()); // Lista vacía

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.NotRecordsFoundInDateRange);
    }

    /// <summary>
    /// Valida que se retornen los cursos con los estudiantes inscritos correctamente.
    /// </summary>
    [Fact]
    public async Task Should_Return_Courses_With_Enrolled_Students_When_Courses_Exist()
    {
        var query = new FindCoursesByDateRangeQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-30),
            DateTo = DateTime.UtcNow
        };
        var courses = new List<Course>
        {
            new Course("Matematicas", 500, 30, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(30)) { Id = 1 },
            new Course("Ciencia", 700, 25, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(40)) { Id = 2 }
        };

        var enrollments = new List<EnrollmentDetailsDto>
        {
            new EnrollmentDetailsDto { CourseId = 1, Student = new StudentDto { Id = 10, FirstName = "Juan", LastName = "Perez" } },
            new EnrollmentDetailsDto { CourseId = 2, Student = new StudentDto { Id = 20, FirstName = "Iron", LastName = "Man" } }
        };

        var expectedDtos = new List<CourseDetailsDto>
        {
            new CourseDetailsDto { Id = 1, Name = "Matematicas", EnrolledStudents = new List<StudentDto> { enrollments[0].Student } },
            new CourseDetailsDto { Id = 2, Name = "Ciencia", EnrolledStudents = new List<StudentDto> { enrollments[1].Student } }
        };

        _courseRepositoryMock.Setup(repo => repo.FindByDateRangeAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(courses);

        _mediatorMock.Setup(m => m.Send(It.Is<GetEnrolledByManyCoursesQuery>(q => q.CourseIds.SequenceEqual(courses.Select(c => c.Id))), It.IsAny<CancellationToken>()))
            .ReturnsAsync(enrollments);

        _mapperMock.Setup(mapper => mapper.Map<CourseDetailsDto>(It.IsAny<Course>()))
            .Returns((Course c) => new CourseDetailsDto { Id = c.Id, Name = c.Name });

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedDtos, options => options.WithoutStrictOrdering());
    }

    /// <summary>
    /// Valida que se retornen los cursos sin estudiantes cuando no hay inscripciones.
    /// </summary>
    [Fact]
    public async Task Should_Return_Courses_Without_Students_When_No_Enrollments_Found()
    {
        var query = new FindCoursesByDateRangeQuery
        {
            DateFrom = DateTime.UtcNow.AddDays(-30),
            DateTo = DateTime.UtcNow
        };
        var courses = new List<Course>
        {
            new Course("Math", 500, 30, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(30)) { Id = 1 }
        };

        _courseRepositoryMock.Setup(repo => repo.FindByDateRangeAsync(query.DateFrom, query.DateTo))
            .ReturnsAsync(courses);

        _mediatorMock.Setup(m => m.Send(It.Is<GetEnrolledByManyCoursesQuery>(q => q.CourseIds.SequenceEqual(courses.Select(c => c.Id))), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EnrollmentDetailsDto>()); // Sin inscripciones

        _mapperMock.Setup(mapper => mapper.Map<CourseDetailsDto>(It.IsAny<Course>()))
            .Returns((Course c) => new CourseDetailsDto { Id = c.Id, Name = c.Name });

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().AllSatisfy(c => c.EnrolledStudents.Should().BeEmpty());
    }
}