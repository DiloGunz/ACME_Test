namespace ACME.CourseManagement.Service.Application.Enrollments.Create;

public record CreateEnrollmentCommand : IRequest<ErrorOr<long>>
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}