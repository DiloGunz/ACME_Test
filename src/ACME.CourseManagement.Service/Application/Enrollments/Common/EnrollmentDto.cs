namespace ACME.CourseManagement.Service.Application.Enrollments.Common;

public record EnrollmentDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}