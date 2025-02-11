using ACME.CourseManagement.Service.Domain.Entities.Students;
using AutoMapper;

namespace ACME.CourseManagement.Service.Application.Students.Common;

/// <summary>
/// Configuración de mapeo para la entidad Student y su DTO StudentDto.
/// Utiliza AutoMapper para definir la conversión entre ambos tipos.
/// </summary>
public class StudentMap : Profile
{
    /// <summary>
    /// Constructor que define los mapeos entre Student y StudentDto.
    /// </summary>
    public StudentMap()
    {
        // Mapea automáticamente las propiedades entre Student y StudentDto.
        CreateMap<Student, StudentDto>();
    }
}
