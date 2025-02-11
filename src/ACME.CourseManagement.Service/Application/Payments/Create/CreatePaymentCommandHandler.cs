using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Interfaces;
using ACME.CourseManagement.Service.Application.Students.GetById;
using ACME.CourseManagement.Service.Domain.Entities.Payments;
using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Application.Payments.Create;

/// <summary>
/// Clase para registrar un Payment
/// </summary>
public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ErrorOr<long>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="paymentRepository"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="mediator"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IMediator mediator, IPaymentGateway paymentGateway)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _paymentGateway = paymentGateway ?? throw new ArgumentNullException(nameof(paymentGateway));
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ErrorOr<long>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // Validar si existe el id del estudiante
        var studentDb = await _mediator.Send(new GetStudentByIdQuery(request.StudentId));
        if (studentDb.IsError)
        {
            return Errors.Student.IdNotFound;
        }

        // validar si existe el curso
        var courseDb = await _mediator.Send(new GetCourseByIdQuery(request.StudentId));
        if (courseDb.IsError)
        {
            return Errors.Course.IdNotFound;
        }

        if (courseDb.Value.EnrollmentFee <= 0)
        {
            return Errors.Payment.PaymentCannotProceedForZeroAmount;
        }

        bool paymentSuccess = await _paymentGateway.ProcessPayment(studentDb.Value.Id, courseDb.Value.Id, courseDb.Value.EnrollmentFee);
        if (!paymentSuccess)
        {
            return Errors.Payment.ProcessFailure;
        }

        var payment = new Payment(request.StudentId, request.CourseId, request.AmountPaid, request.PaymentStatus, request.PaymentDate);
        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
        return payment.Id;
    }
}