namespace ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;

public record GetNumberStudentEnrolledByCourseQuery : IRequest<ErrorOr<int>>
{
    public GetNumberStudentEnrolledByCourseQuery()
    {

    }

    public GetNumberStudentEnrolledByCourseQuery(long courseId)
    {
        CourseId = courseId;
    }

    public long CourseId { get; set; }
}