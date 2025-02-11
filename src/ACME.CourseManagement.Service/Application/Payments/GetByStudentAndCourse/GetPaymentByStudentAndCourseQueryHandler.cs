using ACME.CourseManagement.Service.Application.Payments.Common;
using ACME.CourseManagement.Service.Domain.Entities.Payments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;

public class GetPaymentByStudentAndCourseQueryHandler :
    IRequestHandler<GetPaymentByStudentAndCourseQuery, ErrorOr<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByStudentAndCourseQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<PaymentDto>> Handle(GetPaymentByStudentAndCourseQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByStudentAndCourseAsync(request.StudentId, request.CourseId);
        if (payment is null)
        {
            return Errors.Payment.IdStudentAndIdCoursePaymentNotFound;
        }

        return _mapper.Map<PaymentDto>(payment);
    }
}