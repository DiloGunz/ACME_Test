using ACME.CourseManagement.Service.Application.Payments.Common;
using ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;
using ACME.CourseManagement.Service.Domain.Entities.Payments;
using AutoMapper;
using Moq;

namespace ACME.CourseManagement.UnitTest.Payments.GetByStudentAndCourse;

public class GetPaymentByStudentAndCourseQueryHandlerTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPaymentByStudentAndCourseQueryHandler _handler;

    public GetPaymentByStudentAndCourseQueryHandlerTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPaymentByStudentAndCourseQueryHandler(_paymentRepositoryMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Valida que se retorne un error cuando no se encuentra un pago con el StudentId y CourseId proporcionados.
    /// </summary>
    [Fact]
    public async Task Should_Return_Error_When_Payment_NotFound()
    {
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 1, CourseId = 1 };

        _paymentRepositoryMock.Setup(repo => repo.GetByStudentAndCourseAsync(query.StudentId, query.CourseId))
            .ReturnsAsync((Payment)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Payment.IdStudentAndIdCoursePaymentNotFound);
    }

    /// <summary>
    /// Valida que se retorne un PaymentDto correctamente cuando el pago existe.
    /// </summary>
    [Fact]
    public async Task Should_Return_PaymentDto_When_Payment_Exists()
    {
        // Arrange
        var query = new GetPaymentByStudentAndCourseQuery { StudentId = 1, CourseId = 1 };
        var payment = new Payment { Id = 100, StudentId = 1, CourseId = 1, AmountPaid = 500, PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Completed, PaymentDate = DateTime.UtcNow };
        var paymentDto = new PaymentDto { Id = 100, StudentId = 1, CourseId = 1, AmountPaid = 500, PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Completed, PaymentDate = DateTime.UtcNow };

        _paymentRepositoryMock.Setup(repo => repo.GetByStudentAndCourseAsync(query.StudentId, query.CourseId))
            .ReturnsAsync(payment);

        _mapperMock.Setup(mapper => mapper.Map<PaymentDto>(payment))
            .Returns(paymentDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(paymentDto);
    }
}