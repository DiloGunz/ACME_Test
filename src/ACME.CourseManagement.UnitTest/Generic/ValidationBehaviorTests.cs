using ACME.CourseManagement.Service.Application.Common.Behaviors;
using FluentValidation.Results;
using FluentValidation;
using Moq;

namespace ACME.CourseManagement.UnitTest.Generic;

public class ValidationBehaviorTests
{
    /// <summary>
    /// Prueba que, cuando no hay un validador disponible, la solicitud se procesa normalmente.
    /// </summary>
    [Fact]
    public async Task Handle_NoValidator_ShouldProceed()
    {
        // Arrange
        var request = new TestRequest();
        var nextMock = new Mock<RequestHandlerDelegate<ErrorOr<TestResponse>>>();
        nextMock.Setup(n => n()).ReturnsAsync(new TestResponse());

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<TestResponse>>(null);

        // Act
        var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<TestResponse>();
        nextMock.Verify(n => n(), Times.Once);
    }

    /// <summary>
    /// Prueba que, cuando el validador valida correctamente la solicitud, la solicitud se procesa normalmente.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ShouldProceed()
    {
        // Arrange
        var request = new TestRequest();
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult()); // Sin errores

        var nextMock = new Mock<RequestHandlerDelegate<ErrorOr<TestResponse>>>();
        nextMock.Setup(n => n()).ReturnsAsync(new TestResponse());

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<TestResponse>>(validatorMock.Object);

        // Act
        var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<TestResponse>();
        nextMock.Verify(n => n(), Times.Once);
    }

    /// <summary>
    /// Prueba que, cuando el validador encuentra errores, la respuesta contiene los errores de validación.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidRequest_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = new TestRequest();
        var errors = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error message 1"),
            new ValidationFailure("Property2", "Error message 2")
        };

        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult(errors));

        var nextMock = new Mock<RequestHandlerDelegate<ErrorOr<TestResponse>>>();

        var behavior = new ValidationBehavior<TestRequest, ErrorOr<TestResponse>>(validatorMock.Object);

        // Act
        var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        //result.ErrorsOrEmpty.Should().HaveCount(2);
        //result.ErrorsOrEmpty.First().Code.Should().Be("Validation");
        //result.ErrorsOrEmpty.First().Description.Should().Be("Error message 1");
        //result.ErrorsOrEmpty.Last().Description.Should().Be("Error message 2");

        nextMock.Verify(n => n(), Times.Never);
    }

    // Clases de prueba corregidas
    public class TestRequest : IRequest<ErrorOr<TestResponse>> { }

    public class TestResponse : IErrorOr
    {
        public bool IsError => false;
        public List<Error>? Errors => new();
    }
}