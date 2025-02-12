using ACME.CourseManagement.Service.Application.Courses.Common;
using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Interfaces;
using ACME.CourseManagement.Service.Application.Payments.Create;
using ACME.CourseManagement.Service.Application.Students.Common;
using ACME.CourseManagement.Service.Application.Students.GetById;
using ACME.CourseManagement.Service.Domain.Entities.Payments;
using ACME.CourseManagement.Service.Domain.Primitives;
using ACME.CourseManagement.Service.Infraestructure.Services;
using FluentAssertions;
using Moq;

namespace ACME.CourseManagement.UnitTest.Payments.Create;

public class CreatePaymentHandlerTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPaymentGateway> _paymentGatewayMock;
    private readonly CreatePaymentCommandHandler _handler;

    public CreatePaymentHandlerTests()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mediatorMock = new Mock<IMediator>();
        _paymentGatewayMock = new Mock<IPaymentGateway>();

        _handler = new CreatePaymentCommandHandler(
            _paymentRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mediatorMock.Object,
            _paymentGatewayMock.Object
        );
    }

    /// <summary>
    /// Devuelve error si el estudiante no existe.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_Student_NotFound()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Student.IdNotFound);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Student.IdNotFound);
    }

    /// <summary>
    /// Devuelve error si el curso no existe.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_Course_NotFound()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };

        var student = new StudentDto { Id = 1, FirstName = "Juan", LastName= "Perez", Age=20, DocumentNumber = "123" };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Errors.Course.IdNotFound);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Course.IdNotFound);
    }

    /// <summary>
    /// Devuelve error si el EnrollmentFee del curso es 0
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_EnrollmentFee_Is_Zero()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };
        var student = new StudentDto { Id = 1, FirstName = "Juan", LastName = "Perez", Age = 20, DocumentNumber = "123" };
        var course = new CourseDto { Id = 1, Name = "Matematicas", EnrollmentFee = 0, Capacity = 100, Enable = true, StartDate = DateTime.UtcNow.AddMonths(1), EndDate =  DateTime.UtcNow.AddMonths(1)};

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Payment.PaymentCannotProceedForZeroAmount);
    }

    /// <summary>
    /// Devuelve error si el pago falla.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_Error_When_Payment_Processing_Fails()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };
        var student = new StudentDto { Id = 1, FirstName = "Juan", LastName = "Perez", Age = 20, DocumentNumber = "123" };
        var course = new CourseDto { Id = 1, Name = "Matematicas", EnrollmentFee = 100, Capacity = 100, Enable = true, StartDate = DateTime.UtcNow.AddMonths(1), EndDate = DateTime.UtcNow.AddMonths(1) };

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _paymentGatewayMock.Setup(pg => pg.ProcessPayment(student.Id, course.Id, course.EnrollmentFee))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Payment.ProcessFailure);
    }

    /// <summary>
    /// Devuelve el Id del pago si todo es exitoso.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_PaymentId_When_Payment_Is_Successful()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };
        var student = new StudentDto { Id = 1, FirstName = "Juan", LastName = "Perez", Age = 20, DocumentNumber = "123" };
        var course = new CourseDto { Id = 1, Name = "Matematicas", EnrollmentFee = 100, Capacity = 100, Enable = true, StartDate = DateTime.UtcNow.AddMonths(1), EndDate = DateTime.UtcNow.AddMonths(1) };
        var payment = new Payment(command.StudentId, command.CourseId, command.AmountPaid, command.PaymentStatus, command.PaymentDate);

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _paymentGatewayMock.Setup(pg => pg.ProcessPayment(student.Id, course.Id, course.EnrollmentFee))
            .ReturnsAsync(true);

        _paymentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Payment>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(default)).Returns(Task.FromResult(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Devuelve el Id del pago si todo es exitoso usando implementacion de pasarela de pago de prueba
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Return_PaymentId_When_Payment_Is_Successful_Fake()
    {
        var command = new CreatePaymentCommand()
        {
            CourseId = 1,
            StudentId = 1,
            AmountPaid = 100,
            PaymentStatus = Service.Domain.DomainEnums.Enums.PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
        };
        var student = new StudentDto { Id = 1, FirstName = "Juan", LastName = "Perez", Age = 20, DocumentNumber = "123" };
        var course = new CourseDto { Id = 1, Name = "Matematicas", EnrollmentFee = 100, Capacity = 100, Enable = true, StartDate = DateTime.UtcNow.AddMonths(1), EndDate = DateTime.UtcNow.AddMonths(1) };
        var payment = new Payment(command.StudentId, command.CourseId, command.AmountPaid, command.PaymentStatus, command.PaymentDate);

        _mediatorMock.Setup(m => m.Send(It.Is<GetStudentByIdQuery>(q => q.Id == command.StudentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _mediatorMock.Setup(m => m.Send(It.Is<GetCourseByIdQuery>(q => q.CourseId == command.CourseId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _paymentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Payment>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(default)).Returns(Task.FromResult(1));

        IPaymentGateway paymentGateway = new StripePaymentGateway();
        var handler = new CreatePaymentCommandHandler(_paymentRepositoryMock.Object, _unitOfWorkMock.Object, _mediatorMock.Object, paymentGateway);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeGreaterThan(0);
    }
}