using ACME.CourseManagement.Service.Application.Students.Common;

namespace ACME.CourseManagement.Service.Application.Enrollments.Common;

public record EnrollmentDetailsDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public StudentDto Student { get; set; }
}
