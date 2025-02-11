using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.GetById;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using AutoMapper;
using FluentAssertions;
using Moq;
using ACME.CourseManagement.Service.Domain.Entities.Students;

namespace ACME.CourseManagement.UnitTest.Students.GetById;

public class GetStudentByIdQueryHandlerTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetStudentByIdQueryHandler _handler;

    public GetStudentByIdQueryHandlerTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetStudentByIdQueryHandler(_studentRepositoryMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Verifica que si el estudiante existe, el QueryHandler devuelva correctamente un StudentDto.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_StudentDto_When_Student_Exists()
    {
        var query = new GetStudentByIdQuery { Id = 1 };
        var student = new Student { Id = 1, FirstName = "Juan" };
        var studentDto = new StudentDto { Id = 1, FirstName = "Juan" };

        _studentRepositoryMock.Setup(repo => repo.GetAsync(query.Id))
            .ReturnsAsync(student);

        _mapperMock.Setup(mapper => mapper.Map<StudentDto>(student))
            .Returns(studentDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(studentDto);
    }

    /// <summary>
    /// Prueba que si el estudiante no se encuentra en el repositorio, el QueryHandler devuelva el error Errors.Student.IdNotFound
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_Student_Does_Not_Exist()
    {
        var query = new GetStudentByIdQuery { Id = 999 };

        _studentRepositoryMock.Setup(repo => repo.GetAsync(query.Id))
            .ReturnsAsync((Student)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Student.IdNotFound);
    }

}