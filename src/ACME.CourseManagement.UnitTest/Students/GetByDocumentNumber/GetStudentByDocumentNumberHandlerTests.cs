using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;
using ACME.CourseManagement.Service.Domain.DomainErrors;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace ACME.CourseManagement.UnitTest.Students.GetByDocumentNumber;

public class GetStudentByDocumentNumberHandlerTests
{
    private readonly IMapper _mapper;

	public GetStudentByDocumentNumberHandlerTests()
	{
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new StudentMap()));
        _mapper = config.CreateMapper();
	}

    [Fact]
    public async Task Should_Return_Student_When_Exist_DocumentNumber()
    {
        var studentRepositoryMock = new Mock<IStudentRepository>();
        studentRepositoryMock.Setup(r => r.GetByDocumentNumberAsync("123"))
            .ReturnsAsync(new Student("Juan", "Perez", 20, "123"));

        var handler = new GetStudentByDocumentNumberQueryHandler(studentRepositoryMock.Object, _mapper);
        var query = new GetStudentByDocumentNumberQuery() { DocumentNumber = "123"};

        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<StudentDto>();
        result.Value.FirstName.Should().Be("Juan");
    }

    [Fact]
    public async Task Should_Return_Error_When_Not_Exist_DocumentNumber()
    {
        var studentRepositoryMock = new Mock<IStudentRepository>();
        studentRepositoryMock.Setup(r => r.GetByDocumentNumberAsync("123"))
            .ReturnsAsync(new Student("Juan", "Perez", 20, "123"));

        var handler = new GetStudentByDocumentNumberQueryHandler(studentRepositoryMock.Object, _mapper);
        var query = new GetStudentByDocumentNumberQuery() { DocumentNumber = "1234" };

        var result = await handler.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.NotFound);
        result.FirstError.Code.Should().Be(Errors.Student.NotExistDocumentNumber.Code);
        result.FirstError.Description.Should().Be(Errors.Student.NotExistDocumentNumber.Description);
    }
}