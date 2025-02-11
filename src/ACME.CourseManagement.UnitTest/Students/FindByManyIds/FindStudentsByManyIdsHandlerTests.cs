using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.FindByManyIds;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using AutoMapper;
using FluentAssertions;
using Moq;
using ErrorOr;
using ACME.CourseManagement.Service.Domain.Entities.Students;

namespace ACME.CourseManagement.UnitTest.Students.FindByManyIds;

public class FindStudentsByManyIdsHandlerTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindStudentsByManyIdsQueryHandler _handler;

    public FindStudentsByManyIdsHandlerTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindStudentsByManyIdsQueryHandler(_studentRepositoryMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Devuelve una lista de StudentDto cuando se encuentran estudiantes
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Mapped_Students_When_Students_Exist()
    {
        var studentIds = new List<long> { 1, 2, 3 };
        var students = new List<Student> { new Student { Id = 1 }, new Student { Id = 2 } };
        var studentDtos = new List<StudentDto> { new StudentDto { Id = 1 }, new StudentDto { Id = 2 } };

        _studentRepositoryMock.Setup(repo => repo.FindByManyIdsAsync(studentIds))
            .ReturnsAsync(students);

        _mapperMock.Setup(mapper => mapper.Map<List<StudentDto>>(students))
            .Returns(studentDtos);

        var query = new FindStudentsByManyIdsQuery(studentIds);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Value.Should().BeEquivalentTo(studentDtos);
    }

    /// <summary>
    /// Devuelve un error cuando no se encuentran registros
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_No_Students_Found()
    {
        var studentIds = new List<long> { 10, 20, 30 };
        _studentRepositoryMock.Setup(repo => repo.FindByManyIdsAsync(studentIds))
            .ReturnsAsync(new List<Student>());

        var query = new FindStudentsByManyIdsQuery(studentIds);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be(Errors.Student.NotRecordsFoundByManyIds.Code);
        result.FirstError.Description.Should().Be(Errors.Student.NotRecordsFoundByManyIds.Description);
    }
}