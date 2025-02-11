using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Enrollments.Common;

public class EnrollmentMap : Profile
{
	public EnrollmentMap()
	{
		CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<Enrollment, EnrollmentDetailsDto>();
    }
}