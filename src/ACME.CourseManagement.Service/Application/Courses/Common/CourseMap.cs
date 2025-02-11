using ACME.CourseManagement.Service.Domain.Entities.Courses;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Courses.Common;

/// <summary>
/// Mapea la información de Course a CourseDTO
/// </summary>
public class CourseMap : Profile
{
	public CourseMap()
	{
		CreateMap<Course, CourseDto>();
        CreateMap<Course, CourseDetailsDto>();
    }
}