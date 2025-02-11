using ACME.CourseManagement.Service.Application.Students.Common;

namespace ACME.CourseManagement.Service.Application.Students.GetByDocumentNumber;

public record GetStudentByDocumentNumberQuery : IRequest<ErrorOr<StudentDto>>
{
    public string DocumentNumber { get; set; }
}