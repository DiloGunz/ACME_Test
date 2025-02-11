using ACME.CourseManagement.Service.Domain.Entities.Payments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Payments.Common;

public class PaymentMap : Profile
{
	public PaymentMap()
	{
		CreateMap<Payment, PaymentDto>();
	}
}